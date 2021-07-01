using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Interfaces;
using ARchGLCloud.Domain.Core.Repositories;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Domain.MPP.Interfaces;
using AutoMapper;
using ARchGLCloud.Domain.MPP.Events;
using ARchGLCloud.Application.MPP.Dtos;

namespace ARchGLCloud.Application.MPP.Services
{
    public class MppServiceHelper
    {
        private readonly IMediatorHandler _bus;
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        private readonly IProjectRepository _proRepo;
        private readonly ITaskRepository _taskRepo;
        private readonly ITaskPredecessorLinkRepository _taskLinkRepo;
        private readonly ITaskBaselineRepository _taskBaselineRepo;
        private readonly ITaskExtendedAttributeRepository _taskAttrsRepo;
        private readonly IExtendedAttributeRepository _extAttrsRepo;
        private readonly ICalendarRepository _calRepo;
        private readonly ICalendarWeekDayRepository _calWeekDayRepo;
        private readonly ICalendarExceptionRepository _calExceptionRepo;
        private readonly IResourceRepository _resRepo;
        private readonly IAssignmentRepository _assRepo;

        public MppServiceHelper(IMediatorHandler bus,
                                IMapper mapper,
                                IUnitOfWork uow,
                                ILogger<MppServiceHelper> logger,
                                IProjectRepository proRepo,
                                ITaskRepository taskRepo,
                                ITaskPredecessorLinkRepository taskLinkRepo,
                                ITaskExtendedAttributeRepository taskAttrsRepo,
                                ITaskBaselineRepository taskBaselineRepo,
                                IExtendedAttributeRepository extAttrsRepo,
                                ICalendarRepository calRepo,
                                ICalendarWeekDayRepository calWeekDayRepo,
                                ICalendarExceptionRepository calExceptionRepo,
                                IResourceRepository resRepo,
                                IAssignmentRepository assRepo)
        {
            _bus = bus;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;

            _proRepo = proRepo;
            _extAttrsRepo = extAttrsRepo;
            _calRepo = calRepo;
            _calWeekDayRepo = calWeekDayRepo;
            _calExceptionRepo = calExceptionRepo;
            _resRepo = resRepo;
            _assRepo = assRepo;

            _taskRepo = taskRepo;
            _taskLinkRepo = taskLinkRepo;
            _taskAttrsRepo = taskAttrsRepo;
            _taskBaselineRepo = taskBaselineRepo;
        }

        /// <summary>
        ///   return MppProject from Id, throw exception otherwise
        /// </summary>
        public Project ProjectById(Guid id)
        {
            Project project = _proRepo.Find(id);

            if (project == null)
            {
                _bus.RaiseEvent(new DomainNotification("FindAllByProject", $"Could not find project UUID: {id}"));
                return null;
            }

            return project;
        }

        private IEnumerable<TEntity> Entities<TRepo, TEntity>(TRepo repo, string ids) where TRepo : IRepository<TEntity, Guid> where TEntity : MppAggregateRoot<Guid>
        {
            if (String.IsNullOrWhiteSpace(ids))
                return new List<TEntity>();

            string[] guids = ids.Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (guids.Length == 0)
            {
                return new List<TEntity>();
            }

            return repo.Where(e => guids.Contains(e.Id.ToString()));
        }

        public IEnumerable<Project> GetProjectsByPID(Guid parentId)
        {
            return _proRepo.Where(e => e.ParentId == parentId);
        }

        public Project GetProject(Guid projectId)
        {
            return _proRepo.Find(projectId);
        }

        public IEnumerable<Task> GetTasks(Guid projectId)
        {
            return _taskRepo.Where(e => e.ParentId == projectId).OrderBy(e => e._ID);
        }

        public Task GetTask(Guid taskId)
        {
            return _taskRepo.Find(taskId);
        }

        public IEnumerable<TaskExtendedAttribute> GetTaskExtendedAttributes(Guid taskId)
        {
            return _taskAttrsRepo.Where(e => e.ParentId == taskId);
        }

        public TaskExtendedAttribute GetTaskExtendedAttribute(Guid attrId)
        {
            return _taskAttrsRepo.Find(attrId);
        }

        public IQueryable<TaskPredecessorLink> GetTaskPredecessorLinks(Guid taskId)
        {
            return _taskLinkRepo.Where(e => e.ParentId == taskId);
        }

        public TaskPredecessorLink GetTaskPredecessorLink(Guid linkId)
        {
            return _taskLinkRepo.Find(linkId);
        }

        public IEnumerable<TaskBaseline> GetTaskBaselines(Guid taskId)
        {
            return _taskBaselineRepo.Where(e => e.ParentId == taskId);
        }

        public TaskBaseline GetTaskBaseline(Guid baselineId)
        {
            return _taskBaselineRepo.Find(baselineId);
        }

        public ExtendedAttribute GetExtendedAttribute(Guid attrId)
        {
            return _extAttrsRepo.Find(attrId);
        }

        

        public IEnumerable<ExtendedAttribute> GetExtendedAttributes(Guid projectId)
        {
            return _extAttrsRepo.Where(e => e.ParentId == projectId).OrderBy(e => e.FieldID);
        }

        public IQueryable<Calendar> GetCalendars(Guid projectId)
        {
            return _calRepo.Where(e => e.ParentId == projectId);
        }

        public IQueryable<CalendarWeekDay> GetCalendarWeekDaysByParentId(Guid calendarId)
        {
            return _calWeekDayRepo.Where(e => e.ParentId == calendarId);
        }

        public void UpdateCalendarWeekDays(IQueryable<CalendarWeekDay> calendarWeeks)
        {
            _calWeekDayRepo.UpdateAll(calendarWeeks);
        }

        public void AddCalendarWeekDays(CalendarWeekDay calendarWeek)
        {
            _calWeekDayRepo.Add(calendarWeek);
        }

        public IQueryable<CalendarException> GetCalendarExceptionsByParentId(Guid calendarId)
        {
            return _calExceptionRepo.Where(e => e.ParentId == calendarId);
        }

        public CalendarException GetCalendarException(Guid exceptionId)
        {
            return _calExceptionRepo.Find(exceptionId);
        }

        public void AddCalendarException(CalendarException input)
        {
            _calExceptionRepo.Add(input);
        }

        public void DeleteCalendarException(Guid id)
        {
            CalendarException exception = _calExceptionRepo.Find(id);
            // Calendar calendar = _calRepo.Find(exception.ParentId);
            // calendar.ExceptionsUUIDs = RemoveForeignKey(exception.Id, calendar.ExceptionsUUIDs);
            // _calRepo.Update(calendar);
            //_bus.RaiseEvent(_mapper.Map<CalendarUpdatedEvent>(calendar));
            _calExceptionRepo.Delete(id);
            _bus.RaiseEvent(_mapper.Map<CalendarExceptionRemovedEvent>(exception));
        }

        public void UpdateCalendarException(CalendarException input)
        {
            _calExceptionRepo.Update(input);
        }

        public IQueryable<Resource> GetResources(Guid projectId)
        {
            return _resRepo.Where(e => e.ParentId == projectId);
        }

        

        public bool IsValidTaskAttribute(Guid projectId, Guid taskId, TaskExtendedAttribute attr)
        {
            // check if fieldID is null
            if (String.IsNullOrWhiteSpace(attr.FieldID))
            {
                _bus.RaiseEvent(new DomainNotification("IsValidTaskAttribute", $"FieldID is MUST fields!"));
                return false;
            }

            Project project = GetProject(projectId);
            if (project == null)
                return false;

            // check if fieldID already in project's attribute list
            bool bFound = false;
            IEnumerable<ExtendedAttribute> extAttrs = GetExtendedAttributes(projectId);
            foreach (var a in extAttrs)
            {
                if (a.FieldID == attr.FieldID)
                {
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                _bus.RaiseEvent(new DomainNotification("IsValidTaskAttribute", $"FieldID {attr.FieldID} not not exist in project!"));
                return false;
            }

            Task task = GetTask(taskId);
            if (task == null)
                return false;

            // check if fieldID already exist in task's attribute list
            IEnumerable<TaskExtendedAttribute> attrs = GetTaskExtendedAttributes(task.Id);
            foreach (var a in attrs)
            {
                if (a.FieldID == attr.FieldID)
                {
                    _bus.RaiseEvent(new DomainNotification("CreateTaskAttribute", $"FieldID {attr.FieldID} already exist!"));
                    return false;
                }
            }

            return true;
        }

        public int GetTaskMaximumUID(Guid projectId)
        {
            IQueryable<Task> tasks = _taskRepo.Where(e => e.ParentId == projectId);

            return tasks.Max(task => task.UID);
        }

        public TEntity CopyEntity<TEntity>(TEntity src, TEntity dest) where TEntity : MppAggregateRoot<Guid>
        {
            PropertyInfo[] props = typeof(TEntity).GetProperties();
            string[] forbidOverwrite = new string[] { "Id", "ParentId", "Enabled" };
            foreach (var prop in props)
            {
                if (forbidOverwrite.Contains(prop.Name))
                    continue;

                if (prop.GetCustomAttribute(typeof(NotMappedAttribute)) != null)
                    continue;

                object value = prop.GetMethod.Invoke(src, null);
                prop.SetMethod.Invoke(dest, new object[] { value });
            }

            return dest;
        }

        public TEntity CopyEntity<TEntity>(JObject src, TEntity dest, string[] forbidFields = null) where TEntity : MppAggregateRoot<Guid>
        {
            IEnumerable<JProperty> jprops = src.Properties();
            string[] forbidOverwrite = new string[] { "Id", "ParentId", "Enabled" };
            foreach (var jprop in jprops)
            {
                if (forbidOverwrite.Contains(jprop.Name))
                {
                    continue;
                }

                if (forbidFields != null && forbidFields.Contains(jprop.Name))
                {
                    continue;
                }

                PropertyInfo prop = dest.GetType().GetProperty(jprop.Name);
                if (prop == null)
                {
                    _bus.RaiseEvent(new DomainNotification("CopyEntity", $"Unknown field: {jprop.Name}"));
                    return null;
                }

                Object value;
                if (jprop.Value.Value<string>() != null)
                {
                    // Convert.ChangeType unable to convert "string" to Nullable<DateTime>
                    if (prop.GetMethod.ReturnType == typeof(Nullable<DateTime>))
                    {
                        value = Convert.ToDateTime(jprop.Value.Value<string>());
                    }
                    else
                    {
                        value = Convert.ChangeType(jprop.Value.Value<string>(), prop.GetMethod.ReturnType);
                    }
                }
                else
                {
                    value = null;
                }

                prop.SetMethod.Invoke(dest, new object[] { value });
            }

            return dest;
        }

        #region Assignment

        public Assignment GetAssignment(Guid id)
        {
            return _assRepo.Find(id);
        }

        public IQueryable<Assignment> GetAssignments(Guid projectId)
        {
            return _assRepo.Where(e => e.ParentId == projectId);
        }

        public void AddProjectAssignment(Assignment ass)
        {
            _assRepo.Add(ass);
            _bus.RaiseEvent(_mapper.Map<AssignmentAddedEvent>(ass));
        }

        public void RemoveProjectAssignment(Assignment ass)
        {
            _assRepo.Delete(ass.Id);
            _bus.RaiseEvent(_mapper.Map<AssignmentRemovedEvent>(ass));
        }

        #endregion

        #region Resource
        public Resource GetResource(Guid id)
        {
            return _resRepo.Find(id);
        }

        public void RemoveProjectResource(Resource ass)
        {
            _resRepo.Delete(ass.Id);
            _bus.RaiseEvent(_mapper.Map<ResourceRemovedEvent>(ass));
        }

        public void AddProjectResource(Resource ass)
        {
            _resRepo.Add(ass);
            _bus.RaiseEvent(_mapper.Map<ResourceAddedEvent>(ass));
        }
        #endregion


        #region Attribute
        public void AddProjectAttribute(ExtendedAttribute attr)
        {
            _extAttrsRepo.Add(attr);
            _bus.RaiseEvent(_mapper.Map<ExtendedAttributeAddedEvent>(attr));


            Project project = _proRepo.Find(attr.ParentId);
            project.ExtendedAttributesUUIDs = AddForeignKey(attr.Id, project.ExtendedAttributesUUIDs);

            _proRepo.Update(project);

            _bus.RaiseEvent(_mapper.Map<ProjectUpdatedEvent>(attr));
        }

        public void RemoveProjectAttribute(ExtendedAttribute attr)
        {
            Project project = _proRepo.Find(attr.ParentId);
            project.ExtendedAttributesUUIDs = RemoveForeignKey(attr.Id, project.ExtendedAttributesUUIDs);

            _proRepo.Update(project);
            _bus.RaiseEvent(_mapper.Map<ProjectUpdatedEvent>(project));

            _extAttrsRepo.Delete(attr.Id);
            _bus.RaiseEvent(_mapper.Map<ExtendedAttributeRemovedEvent>(attr));
        }

        #endregion

        public void UpdateProject(Project project)
        {
            _proRepo.Update(project);
            _bus.RaiseEvent(_mapper.Map<ProjectUpdatedEvent>(project));
        }

        public void DeleteProject(Project project)
        {
            var tasks = _taskRepo.Where(e => e.ParentId == project.Id).ToList();
            foreach (var task in tasks)
            {
                DeleteTask(task);
            }

            var calendars = _calRepo.Where(e => e.ParentId == project.Id).ToList();
            foreach (var cal in calendars)
            {
                _calWeekDayRepo.DeleteAll(_calWeekDayRepo.Where(e => e.ParentId == cal.Id));
            }

            _calRepo.DeleteAll(_calRepo.Where(e => e.ParentId == project.Id));
            _assRepo.DeleteAll(_assRepo.Where(e => e.ParentId == project.Id));
            _resRepo.DeleteAll(_resRepo.Where(e => e.ParentId == project.Id));
            _extAttrsRepo.DeleteAll(_extAttrsRepo.Where(e => e.ParentId == project.Id));

            _proRepo.Delete(project.Id);
            _bus.RaiseEvent(_mapper.Map<ExtendedAttributeUpdatedEvent>(project));
        }

        public void AddTask(Task task)
        {
            _taskRepo.Add(task);
            _bus.RaiseEvent(_mapper.Map<TaskAddedEvent>(task));
        }

        public void UpdateTask(Task task)
        {
            _taskRepo.Update(task);
            _bus.RaiseEvent(_mapper.Map<TaskUpdatedEvent>(task));
        }

        public void UpdateAllTask(IEnumerable<Task> tasks)
        {
            _taskRepo.UpdateAll(tasks);
        }

        public void DeleteTask(Task task)
        {
            Project project = _proRepo.Find(task.ParentId);
            project.TasksUUIDs = RemoveForeignKey(task.Id, project.TasksUUIDs);
            _proRepo.Update(project);

            IQueryable<TaskPredecessorLink> links = _taskLinkRepo.Where(e => e.ParentId == task.Id);
            foreach (var link in links)
            {
                _taskLinkRepo.Delete(link.Id);
            }

            IQueryable<TaskExtendedAttribute> attrs = _taskAttrsRepo.Where(e => e.ParentId == task.Id);
            foreach (var attr in attrs)
            {
                _taskAttrsRepo.Delete(attr.Id);
            }

            IQueryable<TaskBaseline> baselines = _taskBaselineRepo.Where(e => e.ParentId == task.Id);
            foreach (var baseline in baselines)
            {
                _taskBaselineRepo.Delete(baseline.Id);
            }

            _taskRepo.Delete(task.Id);
        }

        public void AddTaskAttribute(TaskExtendedAttribute attr)
        {
            _taskAttrsRepo.Add(attr);

            Task task = _taskRepo.Find(attr.ParentId);
            task.ExtendedAttributeUUIDs = AddForeignKey(attr.Id, task.ExtendedAttributeUUIDs);

            _taskRepo.Update(task);
        }

        public void UpdateTaskAttribute(TaskExtendedAttribute attr)
        {
            _taskAttrsRepo.Update(attr);
        }

        public void DeleteTaskAttribute(TaskExtendedAttribute attr)
        {
            Task task = _taskRepo.Find(attr.ParentId);
            task.ExtendedAttributeUUIDs = RemoveForeignKey(attr.Id, task.ExtendedAttributeUUIDs);

            _taskRepo.Update(task);

            _taskAttrsRepo.Delete(attr.Id);
        }

        public void AddTaskPredecessorLink(TaskPredecessorLink link)
        {
            _taskLinkRepo.Add(link);

            Task task = _taskRepo.Find(link.ParentId);
            task.PredecessorLinkUUIDs = AddForeignKey(link.Id, task.PredecessorLinkUUIDs);

            _taskRepo.Update(task);
        }

        public void UpdateTaskPredecessorLink(TaskPredecessorLink link)
        {
            _taskLinkRepo.Update(link);
        }

        public void DeleteTaskPredecessorLink(TaskPredecessorLink link)
        {
            Task task = _taskRepo.Find(link.ParentId);
            task.PredecessorLinkUUIDs = RemoveForeignKey(link.Id, task.PredecessorLinkUUIDs);

            _taskRepo.Update(task);

            _taskLinkRepo.Delete(link.Id);
        }

        public void AddTaskBaseline(TaskBaseline baseline)
        {
            _taskBaselineRepo.Add(baseline);

            Task task = _taskRepo.Find(baseline.ParentId);
            task.BaselineUUIDs = AddForeignKey(baseline.Id, task.BaselineUUIDs);

            _taskRepo.Update(task);
        }

        public void UpdateTaskBaseline(TaskBaseline baseline)
        {
            _taskBaselineRepo.Update(baseline);
        }

        public void DeleteTaskBaseline(TaskBaseline baseline)
        {
            Task task = _taskRepo.Find(baseline.ParentId);
            task.BaselineUUIDs = RemoveForeignKey(baseline.Id, task.BaselineUUIDs);

            _taskRepo.Update(task);

            _taskBaselineRepo.Delete(baseline.Id);
        }

        public ExtendedAttribute ExtendedAttributeById(Guid id)
        {
            ExtendedAttribute attr = _extAttrsRepo.Find(id);

            if (attr == null)
            {
                throw new Exception($"Failed to find extended attribute UUID: {id}");
            }

            return attr;
        }

        public bool IsValidProject(Guid projectId)
        {
            Project project = GetProject(projectId);
            if (project == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetProject", $"Could not find project {projectId}"));
                return false;
            }

            return true;
        }

        public bool IsValidProjectExtendedAttribute(Guid projectId, Guid attrId)
        {
            ExtendedAttribute attr = GetExtendedAttribute(attrId);
            if (attr == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetExtendedAttribute", $"Could not find attribute {attrId}"));
                return false;
            }

            if (attr.ParentId != projectId)
            {
                _bus.RaiseEvent(new DomainNotification("GetExtendedAttribute", $"attribute {attrId} is not under the project {projectId}"));
                return false;
            }

            return true;
        }

        public bool IsValidTask(Guid projectId, Guid taskId)
        {
            Task task = GetTask(taskId);
            if (task == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetTask", $"Could not find task {taskId}"));
                return false;
            }

            if (task.ParentId != projectId)
            {
                _bus.RaiseEvent(new DomainNotification("GetTask", $"task {taskId} is not under the project {projectId}"));
                return false;
            }

            return true;
        }

        public bool IsValidTaskAttribute(Guid taskId, Guid attrId)
        {
            TaskExtendedAttribute attr = GetTaskExtendedAttribute(attrId);
            if (attr == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskAttribute", $"Could not find attribute {attrId}"));
                return false;
            }

            if (attr.ParentId != taskId)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskAttribute", $"attribute {attrId} is not under task {taskId}"));
                return false;
            }

            return true;
        }

        public bool IsValidTaskPredecessorLink(Guid taskId, Guid linkId)
        {
            TaskPredecessorLink link = GetTaskPredecessorLink(linkId);
            if (link == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskPredecessorLink", $"Could not find link {linkId}"));
                return false;
            }

            if (link.ParentId != taskId)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskPredecessorLink", $"predecessor {linkId} is not under task {taskId}"));
                return false;
            }

            return true;
        }

        public bool IsValidTaskBaseline(Guid taskId, Guid baseId)
        {
            TaskBaseline baseline = GetTaskBaseline(baseId);
            if (baseline == null)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskBaseline", $"Could not find baseline {baseId}"));
                return false;
            }

            if (baseline.ParentId != taskId)
            {
                _bus.RaiseEvent(new DomainNotification("GetTaskBaseline", $"baseline {baseId} is not under task {taskId}"));
                return false;
            }

            return true;
        }

        public void Commit()
        {
            if (!_uow.Commit())
            {
                _bus.RaiseEvent(new DomainNotification("Commit changes", $"Failed to save changes to database, It should be INTERNAL ERROR!!!"));
            }
        }

        public static Guid[] ForeignKeys(string uuids)
        {
            string[] ids = uuids.Split(",");
            Guid[] result = new Guid[ids.Length];

            for (var i = 0; i < ids.Length; i++)
            {
                result[i] = Guid.Parse(ids[i]);
            }

            return result;
        }

        public static bool IsForeignKey(Guid guid, string uuids)
        {
            string[] ids = uuids.Split(",");
            foreach (var id in ids)
            {
                if (id == guid.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public static string AddForeignKey(Guid guid, string uuids)
        {
            if (String.IsNullOrWhiteSpace(uuids))
            {
                return guid.ToString();
            }
            else
            {
                if (IsForeignKey(guid, uuids))
                {
                    return uuids;
                }
                else
                {
                    return uuids += ("," + guid);
                }
            }
        }

        public static string RemoveForeignKey(Guid guid, string uuids)
        {
            if (!IsForeignKey(guid, uuids))
            {
                return uuids;
            }

            string result = String.Empty;
            string[] ids = uuids.Split(",");
            foreach (var id in ids)
            {
                if (id == guid.ToString())
                {
                    continue;
                }
                else
                {
                    if (String.IsNullOrEmpty(result))
                    {
                        result = id.ToString();
                    }
                    else
                    {
                        result += ("," + id);
                    }
                }
            }

            return result;
        }
    }
}
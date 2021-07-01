using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using AutoMapper;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.MPP.Filters;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Interfaces;
using ARchGLCloud.Application.MPP.Dtos;

namespace ARchGLCloud.Application.MPP.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly ILogger _logger;
        private readonly MppServiceHelper _helper;
        private readonly IProjectRepository _projectRepository;
        private readonly ICalendarService _calendarService;

        public ProjectService(IMapper mapper, IMediatorHandler bus, ILogger<ProjectService> logger, MppServiceHelper helper, 
            IProjectRepository projectRepository,
            ICalendarService calendarService
            )
        {
            _mapper = mapper;
            _bus = bus;
            _logger = logger;

            _helper = helper;
            _projectRepository = projectRepository;
            _calendarService = calendarService;
        }

        public void Add(Project model)
        {
            throw new NotImplementedException();
        }

        public void Update(Project model)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Project Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> Pager(ProjectQueryFilter filter, out int count)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region 项目project
        public IEnumerable<Project> GetProjects(Guid parentId)
        {
            IEnumerable<Project> projects = _helper.GetProjectsByPID(parentId);
            if (projects.Count() == 0)
            {
                // _bus.RaiseEvent(new DomainNotification("AddProjects", $"invalid parentId {parentId}"));
                return new List<Project>();
            }

            return projects;
        }

        public GetProjectDto GetProject(Guid projectId)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            Project project = _helper.GetProject(projectId);
            var projectViewModel = _mapper.Map<GetProjectDto>(project);
            List<Calendar> calendars = _calendarService.GetCalendars(projectId).ToList();
            var calendarsViewModel = _mapper.Map<List<CalendarDto>>(calendars);


            projectViewModel.Calendars = calendarsViewModel;
            projectViewModel.Assignments = _helper.GetAssignments(projectId).ToList();
            projectViewModel.Resources = _helper.GetResources(projectId).ToList();
            for (int i = 0; i < projectViewModel.Calendars.Count(); i++)
            {
                var calendar = projectViewModel.Calendars[i];

              calendar.Exceptions = _calendarService.GetCalendarExceptions(calendar.Id);

                calendar.WeekDays = _calendarService.GetCalendarWeekDays(calendar.Id);
                
            }
            return projectViewModel;
        }
      
        public Project UpdateProject(Guid projectId, JObject fields)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            Project project = _projectRepository.Find(projectId); //_helper.GetProject(projectId);
            try
            {
                string[] forbidFields = new string[] {"SaveVersion", "UID", "CreationDate",
                                                  "LastSaved", //"StartDate", "FinishDate",
                                                  "extendedAttributesUUIDs", "calendarsUUIDs",
                                                  "tasksUUIDs", "resourcesUUIDs", "assignmentsUUIDs"};

                Project newProject = _helper.CopyEntity(fields, project, forbidFields);
                if (newProject == null)
                    return null;

                _projectRepository.Update(newProject);
                //_helper.UpdateProject(newProject);
                _helper.Commit();
                return newProject;
            }
            catch (Exception ex)
            {
                _bus.RaiseEvent(new DomainNotification("CreateAttribute", $"update project error"));
                Console.WriteLine("update project error", ex);
            }
            return project;
        }

        public Project DeleteProject(Guid projectId)
        {
            var project = _helper.GetProject(projectId);
            if (project == null)
            {
                return null;
            }

            _helper.DeleteProject(project);
            _helper.Commit();
            return project;
        }
        #endregion

        #region 资源Attributes
        public IEnumerable<ExtendedAttribute> GetAttributes(Guid projectId)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            return _helper.GetExtendedAttributes(projectId);
        }

        private bool IsAttributeExist(Guid projectId, ExtendedAttribute attr)
        {
            IEnumerable<ExtendedAttribute> attrs = _helper.GetExtendedAttributes(projectId);
            foreach (var a in attrs)
            {
                if (a.FieldID == attr.FieldID)
                    return true;
            }

            return false;
        }

        public ExtendedAttribute CreateAttribute(Guid projectId, ExtendedAttribute fields)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            ExtendedAttribute destAttr = new ExtendedAttribute();
            ExtendedAttribute newAttr = _helper.CopyEntity(fields, destAttr);
            if (newAttr == null)
                return null;

            // FIXME
            if (newAttr.FieldID != "188744016" || newAttr.FieldName != "ARCHGL_BINDING_COMPONENTS")
            {
                _bus.RaiseEvent(new DomainNotification("CreateAttribute", $"For this version of API, FieldID should equal 188744016, and FieldName should be ARCHGL_BINDING_COMPONENTS"));
                return null;
            }

            if (IsAttributeExist(projectId, newAttr))
            {
                _bus.RaiseEvent(new DomainNotification("CreateAttribute", $"project already attribute with FieldID: {newAttr.FieldID}"));
                return null;
            }

            newAttr.ParentId = projectId;
            newAttr.UserDef = true;
            _helper.AddProjectAttribute(newAttr);
            _helper.Commit();

            return newAttr;
        }

        public ExtendedAttribute DeleteAttribute(Guid projectId, Guid attrId)
        {
            ExtendedAttribute attr = _helper.GetExtendedAttribute(attrId);
            if (attr == null || attr.ParentId != projectId)
            {
                return null;
            }

            _helper.RemoveProjectAttribute(attr);
            _helper.Commit();

            return attr;
        }
        #endregion

        #region 分配Assignment
        public Assignment CreateAssignment(Guid projectId, JObject fields)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            Assignment destAssign = new Assignment();
            Assignment newAssign = _helper.CopyEntity(fields, destAssign);
            if (newAssign == null)
                return null;

            newAssign.ParentId = projectId;
            _helper.AddProjectAssignment(newAssign);
            _helper.Commit();

            return newAssign;
        }

        public Assignment DeleteAssignment(Guid projectId, Guid assId)
        {
            Assignment ass = _helper.GetAssignment(assId);
            if (ass == null || ass.ParentId != projectId)
            {
                return null;
            }

            _helper.RemoveProjectAssignment(ass);
            _helper.Commit();

            return ass;
        }

        #endregion

        #region 分配Resource

        public Resource CreateResource(Guid projectId, JObject input)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            Resource newResource = new Resource();
            Resource copyResource = _helper.CopyEntity(input, newResource);
            if (copyResource == null)
                return null;

            copyResource.ParentId = projectId;
            _helper.AddProjectResource(copyResource);
            _helper.Commit();

            return copyResource;
        }

        public Resource DeleteResource(Guid projectId, Guid resId)
        {
            Resource res = _helper.GetResource(resId);
            if (res == null || res.ParentId != projectId)
            {
                return null;
            }

            _helper.RemoveProjectResource(res);
            _helper.Commit();

            return res;
        }

        #endregion
    }
}

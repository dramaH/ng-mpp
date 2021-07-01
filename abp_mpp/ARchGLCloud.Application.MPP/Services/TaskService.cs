using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.MPP.Filters;
using ARchGLCloud.Application.MPP.Interfaces;
using Newtonsoft.Json;

namespace ARchGLCloud.Application.MPP.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMediatorHandler _bus;
        private readonly MppServiceHelper _helper;

        public TaskService(IMediatorHandler bus, MppServiceHelper helper)
        {
            _bus = bus;
            _helper = helper;
        }

        public void Add(Task model)
        {
            throw new NotImplementedException();
        }

        public void Update(Task model)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> Pager(TaskQueryFilter filter, out int count)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetAllTasks(Guid projectId)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            try
            {
                var tasks = _helper.GetTasks(projectId).ToList();
                for (int i = 0; i < tasks.Count(); i++)
                {
                    var task = tasks[i];
                    task.PredecessorLink = _helper.GetTaskPredecessorLinks(task.Id).ToList();
                    task.Baseline = _helper.GetTaskBaselines(task.Id).ToList();
                    task.ExtendedAttribute = _helper.GetTaskExtendedAttributes(task.Id).ToList();
                }

                return tasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task GetTask(Guid projectId, Guid taskId)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            Task task = _helper.GetTask(taskId);
            task.PredecessorLink = _helper.GetTaskPredecessorLinks(task.Id).ToList();
            task.Baseline = _helper.GetTaskBaselines(task.Id).ToList();
            task.ExtendedAttribute = _helper.GetTaskExtendedAttributes(task.Id).ToList();

            return task;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Task CreateTask(Guid projectId, JObject fields)
        {
            if (!_helper.IsValidProject(projectId))
                return null;

            // allow user overwrite, helpful for batch operation reference.
            Guid id = Guid.Parse(fields.GetValue("Id")?.Value<string>());
            if (_helper.GetTask(id) != null)
            {
                _bus.RaiseEvent(new DomainNotification("CreateTask", $"Task Id {id} already exist, try use another one!"));
                return null;
            }

            var task = id.Equals(Guid.Empty) ? new Task() : new Task(id);
    
            Task newTask = _helper.CopyEntity(fields, task, new string[] { "CreateDate" });
            if (newTask == null)
                return null;

            if (string.IsNullOrWhiteSpace(newTask.Name))
            {
                _bus.RaiseEvent(new DomainNotification("CreateTask", $"At least MUST specify task Name field."));
                return null;
            }

            if (string.IsNullOrWhiteSpace(newTask.Duration))
            {
                newTask.Duration = "PT0H0M0S";
            }
            else
            {
                if (!IsValidDuration(newTask.Duration))
                {
                    _bus.RaiseEvent(new DomainNotification("CreateTask", $"please check duration field format, expect: PT0H0M0S, got: {newTask.Duration}"));
                    return null;
                }
            }

            newTask.DurationFormat = 7;

            JToken uid = fields.GetValue("UID");
            if (uid == null)
            {
                newTask.UID = _helper.GetTaskMaximumUID(projectId) + 1;
            }
            else
            {
                newTask.UID = uid.Value<int>();
            }

            newTask.CreateDate = DateTime.Now;
            newTask.CalendarUID = -1;
            newTask.ParentId = projectId;
            _helper.AddTask(newTask);
            _helper.Commit();

            return newTask;
        }


        private bool IsValidDuration(string duration)
        {
            string expr = @"PT(\d+)H(\d+)M(\d+)S";
            Match m = Regex.Match(duration, expr, RegexOptions.IgnoreCase);

            if (m.Success)
                return true;

            return false;
        }

        public Task DeleteTask(Guid projectId, Guid taskId)
        {
            Task task = _helper.GetTask(taskId);
            if (task == null || task.ParentId != projectId)
            {
                return null;
            }

            _helper.DeleteTask(task);
            _helper.Commit();

            return task;
        }

        public Task UpdateTask(Guid projectId, Guid taskId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            Task task = _helper.GetTask(taskId);
            Task newTask = _helper.CopyEntity(fields, task, new string[] { "UID", "CreateDate" });
            if (newTask == null)
                return null;

            _helper.UpdateTask(newTask);
            _helper.Commit();

            return newTask;
        }

        public void UpdateAllTask(Guid projectId, JArray tasks)
        {
            if (!_helper.IsValidProject(projectId))
                return;

            List<Task> newTasks = new List<Task>();
            foreach (var t in tasks)
            {
                JObject fields = t as JObject;
                Task task = _helper.GetTask(Guid.Parse(fields["Id"].Value<String>()));
                Task newTask = _helper.CopyEntity(fields, task, new string[] { "UID", "CreateDate" });
                if (newTask == null)
                    return;

                newTasks.Add(newTask);
            }

            _helper.UpdateAllTask(newTasks);
            _helper.Commit();

            return;
        }


        public TaskExtendedAttribute CreateTaskAttribute(Guid projectId, Guid taskId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            TaskExtendedAttribute attr = new TaskExtendedAttribute();
            TaskExtendedAttribute newAttr = _helper.CopyEntity(fields, attr);
            if (newAttr == null)
            {
                return null;
            }

            if (!_helper.IsValidTaskAttribute(projectId, taskId, newAttr))
                return null;

            attr.ParentId = taskId;
            _helper.AddTaskAttribute(newAttr);
            _helper.Commit();

            return newAttr;
        }

        public TaskExtendedAttribute DeleteTaskAttribute(Guid projectId, Guid taskId, Guid attrId)
        {
            Task task = _helper.GetTask(taskId);
            if (task == null || task.ParentId != projectId)
            {
                return null;
            }

            TaskExtendedAttribute attr = _helper.GetTaskExtendedAttribute(attrId);
            if (attr == null || attr.ParentId != taskId)
            {
                return null;
            }

            _helper.DeleteTaskAttribute(attr);
            _helper.Commit();

            return attr;
        }

        public TaskExtendedAttribute UpdateTaskAttribute(Guid projectId, Guid taskId, Guid attrId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            if (!_helper.IsValidTaskAttribute(taskId, attrId))
                return null;

            TaskExtendedAttribute attr = _helper.GetTaskExtendedAttribute(attrId);
            string oldFiledId = attr.FieldID;

            TaskExtendedAttribute newAttr = _helper.CopyEntity(fields, attr);
            if (newAttr == null)
                return null;

            if (newAttr.FieldID != oldFiledId)
            {
                _bus.RaiseEvent(new DomainNotification("UpdateTaskAttribute", $"Change FieldID is not allowed!"));
                return null;
            }

            _helper.UpdateTaskAttribute(newAttr);
            _helper.Commit();

            return newAttr;
        }

        public TaskPredecessorLink CreateTaskPredecessorLink(Guid projectId, Guid taskId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            TaskPredecessorLink link = new TaskPredecessorLink();
            TaskPredecessorLink newLink = _helper.CopyEntity(fields, link);
            if (newLink == null)
            {
                return null;
            }

            // FIXME: check if PredecessorUID already exist!

            newLink.ParentId = taskId;
            _helper.AddTaskPredecessorLink(newLink);
            _helper.Commit();

            return newLink;
        }

        public TaskPredecessorLink DeleteTaskPredecessorLink(Guid projectId, Guid taskId, Guid linkId)
        {
            Task task = _helper.GetTask(taskId);
            if (task == null || task.ParentId != projectId)
            {
                return null;
            }

            TaskPredecessorLink link = _helper.GetTaskPredecessorLink(linkId);
            if (link == null || link.ParentId != taskId)
            {
                return null;
            }

            _helper.DeleteTaskPredecessorLink(link);
            _helper.Commit();

            return link;
        }

        public TaskPredecessorLink UpdateTaskPredecessorLink(Guid projectId, Guid taskId, Guid linkId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            if (!_helper.IsValidTaskPredecessorLink(taskId, linkId))
                return null;

            TaskPredecessorLink link = _helper.GetTaskPredecessorLink(linkId);
            TaskPredecessorLink newLink = _helper.CopyEntity(fields, link);
            if (newLink == null)
                return null;

            // FIXME: Check PredecessorUID

            _helper.UpdateTaskPredecessorLink(newLink);
            _helper.Commit();

            return newLink;
        }

        public TaskBaseline CreateTaskBaseline(Guid projectId, Guid taskId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            TaskBaseline baseline = new TaskBaseline();
            TaskBaseline newBaseline = _helper.CopyEntity(fields, baseline);
            if (newBaseline == null)
            {
                return null;
            }

            newBaseline.ParentId = taskId;
            _helper.AddTaskBaseline(newBaseline);
            _helper.Commit();

            return newBaseline;
        }

        public TaskBaseline DeleteTaskBaseline(Guid projectId, Guid taskId, Guid baselineId)
        {
            Task task = _helper.GetTask(taskId);
            if (task == null || task.ParentId != projectId)
            {
                return null;
            }

            TaskBaseline baseline = _helper.GetTaskBaseline(baselineId);
            if (baseline == null || baseline.ParentId != taskId)
            {
                return null;
            }

            _helper.DeleteTaskBaseline(baseline);
            _helper.Commit();

            return baseline;
        }

        public TaskBaseline UpdateTaskBaseline(Guid projectId, Guid taskId, Guid baselineId, JObject fields)
        {
            if (!_helper.IsValidTask(projectId, taskId))
                return null;

            if (!_helper.IsValidTaskBaseline(taskId, baselineId))
                return null;

            TaskBaseline baseline = _helper.GetTaskBaseline(baselineId);
            TaskBaseline newBaseline = _helper.CopyEntity(fields, baseline);
            if (newBaseline == null)
                return null;

            _helper.UpdateTaskBaseline(newBaseline);
            _helper.Commit();

            return newBaseline;
        }
    }
}

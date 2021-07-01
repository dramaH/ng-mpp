using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Application.MPP.Filters;


namespace ARchGLCloud.Application.MPP.Interfaces
{
    public interface ITaskService : IService<Guid, TaskQueryFilter, Task>
    {
        IEnumerable<Task> GetAllTasks(Guid projectId);
        Task GetTask(Guid projectId, Guid taskId);

        Task CreateTask(Guid projectId, JObject fields);
        Task DeleteTask(Guid projectId, Guid taskId);
        Task UpdateTask(Guid projectId, Guid taskId, JObject fields);
        void UpdateAllTask(Guid projectId, JArray tasks);

        TaskExtendedAttribute CreateTaskAttribute(Guid projectId, Guid taskId, JObject fields);
        TaskExtendedAttribute DeleteTaskAttribute(Guid projectId, Guid taskId, Guid attrId);
        TaskExtendedAttribute UpdateTaskAttribute(Guid projectId, Guid taskId, Guid attrId, JObject fields);

        TaskPredecessorLink CreateTaskPredecessorLink(Guid projectId, Guid taskId, JObject fields);
        TaskPredecessorLink DeleteTaskPredecessorLink(Guid projectId, Guid taskId, Guid linkId);
        TaskPredecessorLink UpdateTaskPredecessorLink(Guid projectId, Guid taskId, Guid linkId, JObject fields);

        TaskBaseline CreateTaskBaseline(Guid projectId, Guid taskId, JObject fields);
        TaskBaseline DeleteTaskBaseline(Guid projectId, Guid taskId, Guid baselineId);
        TaskBaseline UpdateTaskBaseline(Guid projectId, Guid taskId, Guid baselineId, JObject fields);
    }
}
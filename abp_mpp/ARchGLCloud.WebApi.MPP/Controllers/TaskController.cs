using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.Core.Controllers;
using System.Linq;
using ARchGLCloud.Application.Core;

namespace ARchGLCloud.WebApi.MPP.Controllers
{
    /// <summary>
    ///   Mpp Project's tasks controller
    /// </summary>
    [Route("mpp")]
    public class TaskController : ApiController
    {
        private readonly ITaskService _service;
        private readonly IProjectService _projectService;

        public TaskController(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications, IProjectService projectService, ITaskService service) : base(notifications, mediator)
        {
            _service = service;
            _projectService = projectService;
        }
        /// <summary>
        /// Get all tasks
        /// </summary>
        [HttpGet("project/{projectId:guid}/tasks")]
        public IActionResult GetAllTasks(Guid projectId)
        {
            var tasks = _service.GetAllTasks(projectId).ToList();
            return Response(new ResponseResultSet<Task>()
            {
                Success = true,
                Total = tasks.Count,
                Items = tasks
            });
        }
        /// <summary>
        ///   Get task TASKID meta data, include ExtendedAttributes,
        ///   PredecessorLinks, Baselines.
        /// </summary>
        [HttpGet("project/{projectId:guid}/task/{taskId:guid}")]
        public IActionResult GetTask(Guid projectId, Guid taskId)
        {
            var task = _service.GetTask(projectId, taskId);
            return Response(new ResponseResult<Task>()
            {
                Success = true,
                Item = task
            });
        }

        /// <summary>
        ///   Create task by MPPPROJECTID
        /// </summary>
        [HttpPost("project/{projectId:guid}/task")]
        public IActionResult CreateTask(Guid projectId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var task = _service.CreateTask(projectId, fields);
            return Response(new ResponseResult<Task>()
            {
                Success = true,
                Item = task
            });
        }

        /// <summary>
        ///   Delete task TASKID by MPPPROJECTID
        /// </summary>
        [HttpDelete("project/{projectId:guid}/task/{taskId:guid}")]
        public IActionResult DeleteTask(Guid projectId, Guid taskId)
        {
            var task = _service.DeleteTask(projectId, taskId);
            return Response(new ResponseResult<Task>()
            {
                Success = true,
                Item = task
            });
        }

        /// <summary>
        ///   Update task TASKID meta data by MPPPROJECTID
        /// </summary>
        [HttpPut("project/{projectId:guid}/task/{taskId:guid}")]
        public IActionResult PutTask(Guid projectId, Guid taskId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var task = _service.UpdateTask(projectId, taskId, fields);
            return Response(new ResponseResult<Task>()
            {
                Success = true,
                Item = task
            });
        }

        /// <summary>
        ///   Batch create/update/delete data by MPPPROJECTID
        /// </summary>
        [HttpPut("project/{projectId:guid}/batch")]
        public IActionResult PutTasks(Guid projectId, [FromBody] JArray areq)
        {
            if (areq == null)
            {
                NotifyError("INVALIDJSON", "not valid json array input");
                return Response();
            }

            if (areq.Count == 0)
            {
                NotifyError("INVALIDJSON", "json array should not empty!");
                return Response();
            }

            // validate input format and gathering each operations
            var task_creates = new JArray();
            var task_updates = new JArray();
            var task_deletes = new JArray();
            List<KeyValuePair<Guid, JObject>> link_creates = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> link_updates = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> link_deletes = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> resource_creates = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> resource_deletes = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> assignment_deletes = new List<KeyValuePair<Guid, JObject>>();
            List<KeyValuePair<Guid, JObject>> assignment_creates = new List<KeyValuePair<Guid, JObject>>();
            foreach (var r in areq)
            {
                var elt = r as JObject;
                if (elt == null)
                {
                    NotifyError("NOFOUND", "no object found!");
                    return Response();
                }
                else
                {
                    string op = elt.GetValue("op")?.Value<string>();
                    string type = elt.GetValue("type")?.Value<string>();
                    JObject data = elt.GetValue("data")?.Value<JObject>();
                    if (string.IsNullOrWhiteSpace(op) || string.IsNullOrWhiteSpace(type) || data == null)
                    {
                        NotifyError("NOFOUND", "master field name should exist!");
                        return Response();
                    }
                    else
                    {
                        if (type == "task")
                        {
                            switch (op)
                            {
                                case "create":
                                    task_creates.Add(data);
                                    break;
                                case "update":
                                    var u_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(u_id))
                                    {
                                        NotifyError("NOFOUND", "unable to find Id for task for update!");
                                        return Response();
                                    }
                                    else
                                    {
                                        task_updates.Add(data);
                                    }
                                    break;
                                case "delete":
                                    var d_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(d_id))
                                    {
                                        NotifyError("NOFOUND", "unable to find Id for task for deletion!");
                                        return Response();
                                    }
                                    else
                                    {
                                        task_deletes.Add(data);
                                    }
                                    break;
                                default:
                                    NotifyError("NOFOUND", "invalid task operation!");
                                    return Response();
                            }
                        }
                        else if (type == "link")
                        {
                            string strTaskId = data.GetValue("ParentId")?.Value<string>();
                            if (string.IsNullOrWhiteSpace(strTaskId))
                            {
                                NotifyError("NOFOUND", "unable to find ParentId for link type!");
                                return Response();
                            }

                            Guid taskId;
                            if (!Guid.TryParse(strTaskId, out taskId))
                            {
                                NotifyError("NOFOUND", "ParentId is not valid Guid string!");
                                return Response();
                            }

                            switch (op)
                            {
                                case "create":
                                    link_creates.Add(KeyValuePair.Create(taskId, data));
                                    break;
                                case "update":
                                    var u_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(u_id))
                                    {
                                        NotifyError("NOFOUND", "Unable to find Id for link update!");
                                        return Response();
                                    }
                                    else
                                    {
                                        link_updates.Add(KeyValuePair.Create(taskId, data));
                                    }
                                    break;
                                case "delete":
                                    var d_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(d_id))
                                    {
                                        NotifyError("NOFOUND", "Unable to find Id for link deletion!");
                                        return Response();
                                    }
                                    else
                                    {
                                        link_deletes.Add(KeyValuePair.Create(taskId, data));
                                    }
                                    break;
                                default:
                                    NotifyError("NOFOUND", "Invalid link operation type!");
                                    return Response();
                            }
                        }
                        else if (type == "assignment")
                        {
                            string strParentId = data.GetValue("ParentId")?.Value<string>();
                            if (string.IsNullOrWhiteSpace(strParentId))
                            {
                                NotifyError("NOFOUND", "unable to find ParentId for assignment type!");
                                return Response();
                            }

                            Guid parentId;
                            if (!Guid.TryParse(strParentId, out parentId))
                            {
                                NotifyError("NOFOUND", "ParentId is not valid Guid string!");
                                return Response();
                            }

                            switch (op)
                            {
                                case "create":
                                    assignment_creates.Add(KeyValuePair.Create(parentId, data));
                                    break;
                                case "delete":
                                    var d_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(d_id))
                                    {
                                        NotifyError("NOFOUND", "Unable to find Id for assignment deletion!");
                                        return Response();
                                    }
                                    else
                                    {
                                        assignment_deletes.Add(KeyValuePair.Create(parentId, data));
                                    }
                                    break;
                                default:
                                    NotifyError("NOFOUND", "Invalid assignment operation type!");
                                    return Response();
                            }
                        }else if (type == "resource")
                        {
                            string strParentId = data.GetValue("ParentId")?.Value<string>();
                            if (string.IsNullOrWhiteSpace(strParentId))
                            {
                                NotifyError("NOFOUND", "unable to find ParentId for assignment type!");
                                return Response();
                            }

                            Guid parentId;
                            if (!Guid.TryParse(strParentId, out parentId))
                            {
                                NotifyError("NOFOUND", "ParentId is not valid Guid string!");
                                return Response();
                            }

                            switch (op)
                            {
                                case "create":
                                    resource_creates.Add(KeyValuePair.Create(parentId, data));
                                    break;
                                case "delete":
                                    var d_id = data.GetValue("Id")?.Value<string>();
                                    if (string.IsNullOrWhiteSpace(d_id))
                                    {
                                        NotifyError("NOFOUND", "Unable to find Id for resource deletion!");
                                        return Response();
                                    }
                                    else
                                    {
                                        resource_deletes.Add(KeyValuePair.Create(parentId, data));
                                    }
                                    break;
                                default:
                                    NotifyError("NOFOUND", "Invalid assignment resource type!");
                                    return Response();
                            }
                        }
                        else
                        {
                            NotifyError("NOFOUND", "Invalid batch type!");
                            return Response();
                        }
                    }
                }
            }

            // process
            foreach (var t in task_creates)
            {
                _service.CreateTask(projectId, (JObject)t);
            }

            foreach (var t in task_deletes)
            {
                var id = ((JObject)t).GetValue("Id").Value<string>();
                _service.DeleteTask(projectId, Guid.Parse(id));
            }

            _service.UpdateAllTask(projectId, task_updates);

            foreach (var l in link_creates)
            {
                _service.CreateTaskPredecessorLink(projectId, l.Key, l.Value);
            }

            foreach (var l in link_deletes)
            {
                var id = Guid.Parse(l.Value.GetValue("Id").Value<string>());
                _service.DeleteTaskPredecessorLink(projectId, l.Key, id);
            }

            foreach (var l in link_updates)
            {
                var id = Guid.Parse(l.Value.GetValue("Id").Value<string>());
                _service.UpdateTaskPredecessorLink(projectId, l.Key, id, l.Value);
            }

            foreach (var l in assignment_creates)
            {
                _projectService.CreateAssignment(projectId, l.Value);
            }
            foreach (var l in assignment_deletes)
            {
                var id = Guid.Parse(l.Value.GetValue("Id").Value<string>());
                _projectService.DeleteAssignment(projectId, id);
            }
            
            foreach (var l in resource_creates)
            {
                _projectService.CreateResource(projectId, l.Value);
            }

            foreach (var l in resource_deletes)
            {
                var id = Guid.Parse(l.Value.GetValue("Id").Value<string>());
                _projectService.DeleteResource(projectId, id);
            }

            return Response(new ResponseResult<JArray>()
            {
                Success = true,
                Item = areq
            });
        }

        /// <summary>
        ///   Create attribute of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPost("project/{projectId:guid}/task/{taskId:guid}/extendedattribute")]
        public IActionResult PostAttribute(Guid projectId, Guid taskId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var attr = _service.CreateTaskAttribute(projectId, taskId, fields);
            return Response(new ResponseResult<TaskExtendedAttribute>()
            {
                Success = true,
                Item = attr
            });
        }

        /// <summary>
        ///   Delete attribute ATTRID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpDelete("project/{projectId:guid}/task/{taskId:guid}/extendedattribute/{attrId:guid}")]
        public IActionResult DeleteAttribute(Guid projectId, Guid taskId, Guid attrId)
        {
            var attr = _service.DeleteTaskAttribute(projectId, taskId, attrId);
            return Response(new ResponseResult<TaskExtendedAttribute>()
            {
                Success = true,
                Item = attr
            });
        }

        /// <summary>
        ///   Update attribute ATTRID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPut("project/{projectId:guid}/task/{taskId:guid}/extendedattribute/{attrId:guid}")]
        public IActionResult PutAttribute(Guid projectId, Guid taskId, Guid attrId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var attr = _service.UpdateTaskAttribute(projectId, taskId, attrId, fields);
            return Response(new ResponseResult<TaskExtendedAttribute>()
            {
                Success = true,
                Item = attr
            });
        }

        /// <summary>
        ///   Create predecessor link of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPost("project/{projectId:guid}/task/{taskId:guid}/predecessorlink")]
        public IActionResult PostLink(Guid projectId, Guid taskId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var link = _service.CreateTaskPredecessorLink(projectId, taskId, fields);
            return Response(new ResponseResult<TaskPredecessorLink>()
            {
                Success = true,
                Item = link
            });
        }

        /// <summary>
        ///   Delete predecessor link LINKID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpDelete("project/{projectId:guid}/task/{taskId:guid}/predecessorlink/{linkId:guid}")]
        public IActionResult DeleteLink(Guid projectId, Guid taskId, Guid linkId)
        {
            var link = _service.DeleteTaskPredecessorLink(projectId, taskId, linkId);
            return Response(new ResponseResult<TaskPredecessorLink>()
            {
                Success = true,
                Item = link
            });
        }

        /// <summary>
        ///   Update predecessor link LINKID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPut("project/{projectId:guid}/task/{taskId:guid}/predecessorlink/{linkId:guid}")]
        public IActionResult PutLink(Guid projectId, Guid taskId, Guid linkId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var link = _service.UpdateTaskPredecessorLink(projectId, taskId, linkId, fields);
            return Response(new ResponseResult<TaskPredecessorLink>()
            {
                Success = true,
                Item = link
            });
        }

        /// <summary>
        ///   Create baseline of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPost("project/{projectId:guid}/task/{taskId:guid}/baseline")]
        public IActionResult PostBaseline(Guid projectId, Guid taskId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var baseline = _service.CreateTaskBaseline(projectId, taskId, fields);
            return Response(new ResponseResult<TaskBaseline>()
            {
                Success = true,
                Item = baseline
            });
        }

        /// <summary>
        ///   Delete baseline BASEID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpDelete("project/{projectId:guid}/task/{taskId:guid}/baseline/{baseId:guid}")]
        public IActionResult DeleteBaseline(Guid projectId, Guid taskId, Guid baseId)
        {
            var baseline = _service.DeleteTaskBaseline(projectId, taskId, baseId);
            return Response(new ResponseResult<TaskBaseline>()
            {
                Success = true,
                Item = baseline
            });
        }

        /// <summary>
        ///   Update baseline BASEID of task TASKID by MPPPROJECTID
        /// </summary>
        [HttpPut("project/{projectId:guid}/task/{taskId:guid}/baseline/{baseId:guid}")]
        public IActionResult PutBaseline(Guid projectId, Guid taskId, Guid baseId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var baseline = _service.UpdateTaskBaseline(projectId, taskId, baseId, fields);
            return Response(new ResponseResult<TaskBaseline>()
            {
                Success = true,
                Item = baseline
            });
        }
    }
}
using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.Core.Controllers;
using ARchGLCloud.Application.Core;
using System.Linq;
using ARchGLCloud.Application.MPP.Dtos;

namespace ARchGLCloud.WebApi.MPP.Controllers
{
    /// <summary>
    ///   Mpp Project's controller
    /// </summary>
    [Route("mpp")]
    public class ProjectController : ApiController
    {
        private readonly IProjectService _service;
        private readonly IImportExportService _importer;

        public ProjectController(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications, IProjectService service, IImportExportService importer) : base(notifications, mediator)
        {
            _service = service;
            _importer = importer;
        }
        /// <summary>
        /// 创建项目，parentId为自定义父级id
        /// </summary>
        [HttpPost("project/{parentId:guid}")]
        public IActionResult CreateProject(Guid parentId, [FromBody] CreateProjectDto input)
        {
            var templateFilePath = _importer.GetTemplateFile();

            Project project;
            if (input.Id != null)
            {
                project = new Project(input.Id.Value);
            }
            else
            {
                project = new Project();
            }
            _importer.Xml2Model(templateFilePath, project);
            project.ParentId = parentId;

            //PropertyInfo[] infos = project.GetType().GetProperties();
            //foreach (var info in infos)
            //{
            //    JProperty prop = input[info.Name];
            //    if (prop != null)
            //    {
            //        var value = prop.Value.ToObject(info.GetMethod.ReturnType);
            //        info.SetMethod.Invoke(project, new object[] { value });
            //    }
            //}

            project.Title = input.Title;
            project.StartDate = input.StartDate;
            project.FinishDate = input.FinishDate;
            project.CreationDate = DateTime.Now;
            project.LastSaved = DateTime.Now;
            project.CurrentDate = DateTime.Now;
            Console.WriteLine(project.Id);
            _importer.SaveAll(project);

            var p = _service.GetProject(project.Id);
            return Response(new ResponseResult<GetProjectDto>()
            {
                Success = true,
                Item = p
            });
        }
        /// <summary>
        ///   通过parentId查询所有项目
        /// </summary>
        [HttpGet("{parentId:guid}/projects")]
        public IActionResult GetProjects(Guid parentId)
        {
            var projects = _service.GetProjects(parentId).ToList();
            return Response(new ResponseResultSet<Project>()
            {
                Success = true,
                Total = projects.Count,
                Items = projects
            });
        }
        /// <summary>
        ///   获取项目信息
        /// </summary>
        [HttpGet("project/{projectId:guid}")]
        public IActionResult GetProject(Guid projectId)
        {
            var project = _service.GetProject(projectId);
            return Response(new ResponseResult<GetProjectDto>()
            {
                Success = true,
                Item = project
            });
        }
        /// <summary>
        ///   更新项目
        /// </summary>
        [HttpPut("project/{projectId:guid}")]
        public IActionResult UpdateProject(Guid projectId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var project = _service.UpdateProject(projectId, fields);
            return Response(new ResponseResult<Project>()
            {
                Success = true,
                Item = project
            });
        }
        /// <summary>
        ///   删除项目
        /// </summary>
        [HttpDelete("project/{projectId:guid}")]
        public IActionResult DeleteProject(Guid projectId)
        {
            var project = _service.DeleteProject(projectId);
            return Response(new ResponseResult<Project>()
            {
                Success = true,
                Item = project
            });
        }
        
        /// <summary>
        ///  获取项目资源（extendedattributes）
        /// </summary>
        [HttpGet("project/{projectId:guid}/extendedattributes")]
        public IActionResult GetAttributes(Guid projectId)
        {
            var attrs = _service.GetAttributes(projectId).ToList();
            var bindFieldID = attrs.Find(x => x.FieldID == "188744016");
            if(bindFieldID == null)
            {
                var bindFieldParam = new ExtendedAttribute();
                bindFieldParam.FieldID = "188744016";
                bindFieldParam.FieldName = "ARCHGL_BINDING_COMPONENTS";
                _service.CreateAttribute(projectId, bindFieldParam);
            }
            return Response(new ResponseResultSet<ExtendedAttribute>()
            {
                Success = true,
                Total = attrs.Count,
                Items = attrs
            });
        }
        /// <summary>
        ///   创建项目资源
        /// </summary>
        [HttpPost("project/{projectId:guid}/extendedattribute")]
        public IActionResult CreateAttribute(Guid projectId, [FromBody] ExtendedAttribute fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var attr = _service.CreateAttribute(projectId, fields);
            return Response(new ResponseResult<ExtendedAttribute>()
            {
                Success = true,
                Item = attr
            });
        }
        /// <summary>
        ///   删除项目资源
        /// </summary>
        [HttpDelete("project/{projectId:guid}/extendedattribute/{attrId:guid}")]
        public IActionResult DeleteAttribute(Guid projectId, Guid attrId)
        {
            var attr = _service.DeleteAttribute(projectId, attrId);
            return Response(new ResponseResult<ExtendedAttribute>()
            {
                Success = true,
                Item = attr
            });
        }


        /// <summary>
        ///   创建项目分配
        /// </summary>
        [HttpPost("project/{projectId:guid}/assignment")]
        public IActionResult CreateAssignment(Guid projectId, [FromBody] JObject fields)
        {
            if (fields == null)
            {
                NotifyError("FIELDNULL", "Fields can't be null");
                return Response();
            }

            var attr = _service.CreateAssignment(projectId, fields);
            return Response(new ResponseResult<Assignment>()
            {
                Success = true,
                Item = attr
            });
        }


        /// <summary>
        ///   删除项目分配
        /// </summary>
        [HttpDelete("project/{projectId:guid}/assignment/{assId:guid}")]
        public IActionResult DeleteAssignment(Guid projectId, Guid assId)
        {
            var attr = _service.DeleteAssignment(projectId, assId);
            return Response(new ResponseResult<Assignment>()
            {
                Success = true,
                Item = attr
            });
        }

    }
}
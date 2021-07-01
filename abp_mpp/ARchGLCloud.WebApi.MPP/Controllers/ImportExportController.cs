using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Xml;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using ARchGLCloud.Domain.MPP.Models;
using ARchGLCloud.Application.Core.Controllers;
using ARchGLCloud.Application.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ARchGLCloud.Application.MPP.ViewModels;
using ARchGLCloud.Application.MPP.Dtos;

namespace ARchGLCloud.WebApi.MPP.Controllers
{
    /// <summary>
    ///   MS project MPP/MPX import/export support
    /// </summary>
    [Route("mpp")]
    public class MppImportExportController : ApiController
    {
        private readonly IImportExportService _service;

        public MppImportExportController(IImportExportService service, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _service = service;
        }
        /// <summary>
        /// 导出MPP文档 XML 格式
        /// </summary>
        [HttpGet("export/{projectId:guid}")]
        public IActionResult export(Guid projectId)
        {
            XmlDocument doc = _service.Export2Xml(projectId);

            string outputPath = Path.Combine(_service.GetUploadPath(), DateTime.Now.ToString("yyyMMddHHmmss") + "-download.xml");
            doc.Save(outputPath);

            FileStream stream = new FileStream(outputPath, FileMode.Open);
            return File(stream, "application/octet-steam", projectId + ".xml");
        }

        /// <summary>
        /// 导入MPP文档
        /// </summary>
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromForm]ImportMppDto input)
        {
            if (input.file == null)
            {
                NotifyError("FILENOTFOUND", "找不到文件");
                return Response();
            }

            string uploadPath = _service.GetUploadPath();
            string filePath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyMMddHHmmss") + ".mpp");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await input.file.CopyToAsync(stream);
            }

            string xmlFile = _service.XmlFromMpp(filePath);

            if (string.IsNullOrWhiteSpace(xmlFile))
            {
                NotifyError("status", "Conversion failed");
                return Response();
            }
            else
            {
                Project project;
                if(input.Id != null)
                {
                    project = new Project(input.Id.Value);
                }
                else
                {
                    project = new Project();
                }
                _service.TryImport(xmlFile, project);

                return Response(new ResponseResult<Project>()
                {
                    Success = true,
                    Item = project
                });
            }
        }


        /// <summary>
        /// 导入MPP文档
        /// </summary>
        [HttpPost("mppImport")]
        public async Task<IActionResult> MppImport([FromForm]ImportMppDto input)
        {
            if (input.file == null)
            {
                NotifyError("FILENOTFOUND", "找不到文件");
                return Response();
            }

            string uploadPath = _service.GetUploadPath();
            Console.WriteLine(uploadPath);
            string filePath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyMMddHHmmss") + ".mpp");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await input.file.CopyToAsync(stream);
            }

            string xmlFile = _service.XmlFromMpp(filePath);

            if (string.IsNullOrWhiteSpace(xmlFile))
            {
                NotifyError("status", "Conversion failed");
                return Response();
            }
            else
            {
                Project project;
                if (input.Id != null)
                {
                    project = new Project(input.Id.Value);
                }
                else
                {
                    project = new Project();
                }
                _service.TryImport(xmlFile, project);

                return Response(new ResponseResult<Project>()
                {
                    Success = true,
                    Item = project
                });
            }
        }

        /// <summary>
        /// 导入xml文档
        /// </summary>
        [HttpPost("importXml/{parentId:guid}")]
        public async Task<IActionResult> importXml(Guid parentId, IFormFile file)
        {
            if (file == null)
            {
                NotifyError("FILENOTFOUND", "找不到文件");
                return Response();
            }

            string uploadPath = _service.GetUploadPath();
            string xmlFilePath = Path.Combine(uploadPath, DateTime.Now.ToString("yyyMMddHHmmss") + ".xml");

            using (var stream = new FileStream(xmlFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //string xmlFile = _service.XmlFromMpp(filePath);

            if (string.IsNullOrWhiteSpace(xmlFilePath))
            {
                NotifyError("status", "Conversion failed");
                return Response();
            }
            else
            {
                Project project = new Project();
                project.ParentId = parentId;
                _service.TryImport(xmlFilePath, project);
                return Response(new ResponseResult<Project>()
                {
                    Success = true,
                    Item = project
                });
            }
        }


    }
}
using ARchGLCloud.Application.Core;
using ARchGLCloud.Application.Core.Controllers;
using ARchGLCloud.Application.MPP.Dtos;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.Core.Bus;
using ARchGLCloud.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARchGLCloud.WebApi.MPP.Controllers
{
    [Route("mpp/calendar")]
    public class CalendarController : ApiController
    {
        private readonly ICalendarService _service;

        public CalendarController(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications, ICalendarService service) : base(notifications, mediator)
        {
            _service = service;
        }

        /// <summary>
        ///   更新项目例外日期
        /// </summary>
        [HttpPut("updateException")]
        public IActionResult UpdateException([FromBody] List<CalendarExceptionDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    NotifyError("FIELDNULL", "Id can't be null");
                    return Response();
                }

                _service.UpdateException(item);
            }
            
            return Response(new ResponseResult<string>()
            {
                Success = true,
                Item = null
            });
        }

        /// <summary>
        ///   添加项目例外日期
        /// </summary>
        [HttpPost("addException")]
        public IActionResult AddException([FromBody] List<CalendarExceptionDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    NotifyError("FIELDNULL", "Id can't be null");
                    return Response();
                }
                _service.AddException(item);
            }
            

            return Response(new ResponseResult<string>()
            {
                Success = true,
                Item = null
            });
        }

        /// <summary>
        ///   添加项目例外日期
        /// </summary>
        [HttpDelete("deleteException")]
        public IActionResult DeleteException(Guid id)
        {

            if (id == null)
            {
                NotifyError("FIELDNULL", "Id can't be null");
                return Response();
            }

            _service.DeleteException(id);


            return Response(new ResponseResult<string>()
            {
                Success = true,
                Item = null
            });
        }

        /// <summary>
        ///   更新项目工作周
        /// </summary>
        [HttpPut("updateWeekDays/{projectId:guid}")]
        public IActionResult updateWeekDays(Guid projectId, [FromBody] List<CalendarWeekDayDto> input)
        {
            _service.UpdateWeekDays(projectId, input);
            return Response(new ResponseResult<string>()
            {
                Success = true,
                Item = null
            });
        }
    }
}

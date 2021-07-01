using ARchGLCloud.Application.MPP.Dtos;
using ARchGLCloud.Application.MPP.Interfaces;
using ARchGLCloud.Domain.MPP.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARchGLCloud.Application.MPP.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IMapper _mapper;
        private readonly MppServiceHelper _helper;
        public CalendarService(IMapper mapper, MppServiceHelper helper)
        {
            _mapper = mapper;
            _helper = helper;
        }

        public List<CalendarExceptionDto> GetCalendarExceptions(Guid calendarId)
        {
            var exceptions = _helper.GetCalendarExceptionsByParentId(calendarId);
            return _mapper.Map<List<CalendarExceptionDto>>(exceptions);
        }

        public List<CalendarWeekDayDto> GetCalendarWeekDays(Guid calendarId)
        {
            var weekDays = _helper.GetCalendarWeekDaysByParentId(calendarId);
            return _mapper.Map<List<CalendarWeekDayDto>>(weekDays);
        }

        /// <summary>
        /// 获取calendars
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<Calendar> GetCalendars(Guid projectId)
        {
            if (!_helper.IsValidProject(projectId))
                return null;
            var calendars = _helper.GetCalendars(projectId);
            var validCalendars = new List<Calendar>();
            foreach (var c in calendars)
            {
                if (c.UID == 1)
                {
                    validCalendars.Add(c);
                }
            }
            return validCalendars;
        }

        #region 日历 calendars


        public void UpdateException(CalendarExceptionDto input)
        {
            var exception = _helper.GetCalendarException(input.Id);
            if (exception != null)
            {
                exception.Name = input.Name != null ? input.Name : exception.Name;
                exception.FromDate = input.FromDate != null ? input.FromDate : exception.FromDate;
                exception.ToDate = input.ToDate != null ? input.ToDate : exception.ToDate;
            }
            _helper.UpdateCalendarException(exception);
            _helper.Commit();
        }

        public void AddException(CalendarExceptionDto input)
        {
            var newEcp = new CalendarException();
            newEcp.Name = input.Name;
            newEcp.FromDate = input.FromDate;
            newEcp.ToDate = input.ToDate;
            newEcp.ParentId = input.ParentId;

            _helper.AddCalendarException(newEcp);
            _helper.Commit();
        }

        public void DeleteException(Guid id)
        {
            _helper.DeleteCalendarException(id);
            _helper.Commit();
        }

        public void UpdateWeekDays(Guid parentId, List<CalendarWeekDayDto> input)
        {
            var weekDays = _helper.GetCalendarWeekDaysByParentId(parentId).ToList();
            var updateWeekDays = new List<CalendarWeekDay>();
            var createWeekDays = new List<CalendarWeekDay>();
            foreach (var wd in input)
            {
                var finder = weekDays.Find(w => w.Id == wd.Id);
                if (finder != null)
                {
                    finder.DayWorking = wd.DayWorking;
                    updateWeekDays.Add(finder);
                }
                else
                {
                    var newWeekDay = new CalendarWeekDay();
                    newWeekDay.DayType = wd.DayType;
                    newWeekDay.ParentId = parentId;
                    newWeekDay.DayWorking = wd.DayWorking;
                    createWeekDays.Add(newWeekDay);
                }
                
            }
            _helper.UpdateCalendarWeekDays(updateWeekDays.AsQueryable());

            foreach (var item in createWeekDays)
            {
                _helper.AddCalendarWeekDays(item);
            }


            _helper.Commit();

        }

        #endregion


    }
}

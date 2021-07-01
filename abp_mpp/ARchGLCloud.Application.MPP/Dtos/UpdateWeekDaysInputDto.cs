using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class UpdateWeekDaysInputDto
    {
        [Required]
        public Guid CalendarId { get; set; }
        [Required]
        public List<CalendarWeekDayDto> WeekDayList { get; set; }
    }
}

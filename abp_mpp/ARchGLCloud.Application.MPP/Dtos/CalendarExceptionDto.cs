using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class CalendarExceptionDto
    {
        // The beginning of the exception time.
        public DateTime? FromDate { get; set; }
        // The end of the exception time.
        public DateTime? ToDate { get; set; }

        // The name of the exception.
        [MaxLength(512)]
        public string Name { get; set; }

        /// <summary>
        /// 例外日期id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

    }
}

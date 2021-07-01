using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class CreateProjectDto
    {
        public Guid? Id { get; set; }

        public string Author { get; set; }

        public DateTime FinishDate { get; set; }

        public DateTime StartDate { get; set; }

        public string Title { get; set; }

    }
}

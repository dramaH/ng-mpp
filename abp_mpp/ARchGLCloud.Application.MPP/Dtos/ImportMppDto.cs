using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.MPP.Dtos
{
    public class ImportMppDto
    {
        public Guid? Id { get; set; }

        public IFormFile file { get; set; }

    }
}

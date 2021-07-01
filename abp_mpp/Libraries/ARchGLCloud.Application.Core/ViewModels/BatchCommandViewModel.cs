using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Application.Core
{
    public class BatchCommandViewModel<T>
    {
        public List<T> Entities { get; set; }
    }
}

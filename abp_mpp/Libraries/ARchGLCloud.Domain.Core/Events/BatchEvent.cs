using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Domain.Core.Events
{
    public class BatchEvent<T> : Event
    {
        public List<T> Entities { get; set; }
    }
}

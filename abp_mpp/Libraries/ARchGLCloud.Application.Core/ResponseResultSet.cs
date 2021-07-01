using System.Collections.Generic;

namespace ARchGLCloud.Application.Core
{
    public class ResponseResultSet<T> : ResponseResult where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Core
{
    public class OrderParam
    {
        public OrderParam()
        {
        }

        public OrderParam(string propertyName, OrderMethod method)
        {
            this.PropertyName = propertyName;
            this.Method = method;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public OrderMethod Method { get; set; }
    }
}

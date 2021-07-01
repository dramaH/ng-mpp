using ARchGLCloud.Core;

namespace ARchGLCloud.Application.Core.Filters
{
    public class TenantQueryFilter : QueryFilter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 宿主域名
        /// </summary>
        public string Host { get; set; }

        public override OrderParam[] OrderParams { get => new OrderParam[] { new OrderParam("Name", OrderMethod.ASC) }; set => base.OrderParams = value; }
    }
}

using ARchGLCloud.Core;

namespace ARchGLCloud.Application.Core.Filters
{
    public class QueryFilter
    {
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortOrder { get; set; }
        /// <summary>
        /// 默认排序名称
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// 排序参数（根据SortName生成）
        /// </summary>
        public virtual OrderParam[] OrderParams { get; set; }
    }
}

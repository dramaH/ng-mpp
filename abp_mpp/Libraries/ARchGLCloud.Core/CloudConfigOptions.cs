using System;
using System.Collections.Generic;
using System.Text;

namespace ARchGLCloud.Core
{
    public class CloudConfigOptions
    {
        /// <summary>
        /// 认证地址
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 默认跳转地址
        /// </summary>
        public string DefaultRedirectUrl { get; set; }
        /// <summary>
        /// SpiderBim地址
        /// </summary>
        public string SpiderBimUrl { get; set; }
        /// <summary>
        /// SpdView地址
        /// </summary>
        public string SpdViewUrl { get; set; }
        /// <summary>
        /// SpdCostUrl地址
        /// </summary>
        public string SpdCostUrl { get; set; }
        /// <summary>
        /// 文件服务器地址
        /// </summary>
        public string FileServerUrl { get; set; }
        /// <summary>
        /// 身份用户API
        /// </summary>
        public string IdentityApiUrl { get; set; }
        /// <summary>
        /// 租户API地址
        /// </summary>
        public string EngineApiUrl { get; set; }
        /// <summary>
        /// 项目API地址
        /// </summary>
        public string ProjectApiUrl { get; set; }
        /// <summary>
        /// 聊天API地址
        /// </summary>
        public string ChatApiUrl { get; set; }
        /// <summary>
        /// 项目API地址
        /// </summary>
        public string MppApiUrl { get; set; }
        /// <summary>
        /// 默认租户角色值
        /// </summary>
        public string DefaultRoleValue { get; set; } = "23140981-36a8-4260-9f80-8181cf334c5d";
    }
}

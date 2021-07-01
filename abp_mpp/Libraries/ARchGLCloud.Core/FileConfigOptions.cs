namespace ARchGLCloud.Core
{
    public class FileConfigOptions
    {
		//fileServer-------------------------------------------------
		/// <summary>
        /// 解析服务器模型数据库连接(如果为空，会采用配置文件的连接)
        /// </summary>
        public string DbConnectInfo { get; set; }
        /// <summary>
        /// 文件url的前半段，用于生成文件的url
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// 文件放置的物理路径，默认放在wwwroot下
        /// </summary>
        public string BasePath { get; set; }
        /// <summary>
        /// 给解析服务器访问模型文件的路径（注意确保解析服务器有访问权限）
        /// </summary>
        public string IFCPath { get; set; }
        /// <summary>
        /// 给解析服务器放置解析后模型的路径
        /// </summary>
        public string RadonPath { get; set; }
        /// <summary>
        /// 模型转换服务接口
        /// </summary>
        public string ConvertUrl { get; set; }
        /// <summary>
        /// 模型转换进度接口
        /// </summary>
        public string ProgressUrl { get; set; }
		/// <summary>
        /// 获取构件属性接口
        /// </summary>
        public string PropertyUrl { get; set; }
		/// <summary>
        /// 轴网和标高接口
        /// </summary>
        public string GridsAndLevlesUrl { get; set; }
        /// <summary>
        /// 获取属性结构接口
        /// </summary>
        public string ModelTreeApi { get; set; }
        /// <summary>
        /// 获取某模型的所有构件类接口
        /// </summary>
        public string AllLeafTypeApi { get; set; }
        /// <summary>
        /// 获取某模型的所有构件类接口
        /// </summary>
        public string PropertyNamesByType { get; set; }
        /// <summary>
        /// 获取项目模型链接关系接口
        /// </summary>
        public string ModelRelationApi { get; set; }
		/// <summary>
        /// 模型关系接口
        /// </summary>
        public string ModelViewRelationApi { get; set; }
        /// <summary>
        /// 解析后模型所需的额外文件路径
        /// </summary>
        public string ExtraFilePath { get; set; }
        /// <summary>
        /// 文件上传路径
        /// </summary>
        public string FileUploadUrl { get; set; }
		/// <summary>
        /// 不用验证可以直接访问的文件类型
        /// </summary>
        public string AuthIgnoreFile { get; set; }
        /// <summary>
        /// 模型视图接口
        /// </summary>
        public string ModelViewApi { get; set; }
        /// <summary>
        /// 项目主视图接口
        /// </summary>
        public string ProjectViewApi { get; set; }
        /// <summary>
        /// 自定义视图接口
        /// </summary>
        public string CustomViewApi { get; set; }
		/// <summary>
        /// 删除链接模型接口
        /// </summary>
        public string RelationModelDeleteUrl { get; set; }
        /// <summary>
        /// 删除参数化链接模型接口
        /// </summary>
        public string ParameterizationRelationModelDeleteUrl { get; set; }
        /// <summary>
        /// spdv模型转换任务状态修改
        /// </summary>
        public string UpdateModelConvertStateApi { get; set; }
		/// <summary>
        /// 获取某模型的所有构件类型
        /// </summary>
        public string AskSearchInsByPropValueApi { get; set; }
		/// <summary>
        /// 参数化模型转换服务接口
        /// </summary>
        public string ParameterizationConvertUrl { get; set; }
		/// <summary>
        /// 白名单
        /// </summary>
        public string[] WhiteList { get; set; }


        //spdview---------------------------------------------------------------
        /// <summary>
        ///  sso服务访问url
        /// </summary>
        public string SsoUrl { get; set; }
        /// <summary>
        ///  访问identity查询用户url
        /// </summary>
        public string SsoGetUserUrl { get; set; }
		/// <summary>
        /// 分享链接路径
        /// </summary>
        public string ActiveUrl { get; set; }
        /// <summary>
        ///  fileServer 添加模型并开启转换
        /// </summary>
        public string FileServerSaveAndConvertUrl { get; set; }
        /// <summary>
        ///  sso服务访问url
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        ///  sso服务访问url
        /// </summary>
        public string EndTime { get; set; }
		/// <summary>
        /// 活动是否开启通知标记
        /// </summary>
        public bool IsBegin { get; set; }
        /// <summary>
        /// 2d文件类型
        /// </summary>
        public string FileType2d { get; set; }
        /// <summary>
        ///  3d文件类型
        /// </summary>
        public string FileType3d { get; set; }
        /// <summary>
        ///  文档文件类型
        /// </summary>
        public string FileTypeDoc { get; set; }
        /// <summary>
        /// 分享链接路径
        /// </summary>
        public string ShareModelUrl { get; set; }

        public string BaseVisitUrl { get; set; }

        public string AuditModelUrl { get; set; }

        public string ComparemodelUrl { get; set; }

        public string NoValidateOptions { get; set; }

        public string ConnStr { get; set; }
    }
}
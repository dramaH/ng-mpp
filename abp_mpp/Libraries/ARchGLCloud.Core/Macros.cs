namespace ARchGLCloud.Core
{
    public class Macros
    {
#if ENTERPRISE
        public const string Edition_NAME = "企业版";
        public const SPDEdition Edition = SPDEdition.Enterprise;
        public const int MAX_USER = 10001;
#elif EDUCATION
        public const string Edition_NAME = "教育版";
        public const SPDEdition Edition = SPDEdition.Education;
        //预留一个admin和guest账号,实际最大数量为120
        public const int MAX_USER = 122;
#else
        public const string Edition_NAME = "平台版";
        public const SPDEdition Edition = SPDEdition.SaaS;
        public const int MAX_USER = int.MaxValue;
#endif

#if LAN
        public const DeployMode Deploy = DeployMode.LAN;
#else
        public const DeployMode Deploy = DeployMode.INTERNET;
#endif

    }
}
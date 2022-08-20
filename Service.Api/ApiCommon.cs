using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Api
{
    /// <summary>
    /// 基础配置信息
    /// </summary>
    public class ApiCommon
    {
        /// <summary>
        /// Swagger显示的Api Document Name
        /// default: Demo
        /// </summary>
        public static string ApiName = "Demo";

        /// <summary>
        /// default: AllowCors
        /// </summary>
        public static string CorsName = "AllowCors";

        /// <summary>
        /// Swagger显示使用的组件xml文件
        /// </summary>
        public static List<string> XmlDocumentes;

        /// <summary>
        /// 读取配置中的信息节点
        /// </summary>
        public static class Appsetting
        {
            /// <summary>
            /// Api路由前缀,在Control前加统一前缀
            /// 示例:api
            /// 效果:/api/[control]/[route]
            /// </summary>
            public static string ApiRoutePrefix = "Setting:ApiRoutePrefix";

            /// <summary>
            /// 是否开始api的swagger文档,默认关闭
            /// 访问路径:/api/index.html
            /// </summary>
            public static string EnableShowApiSwagger = "Setting:EnableShowApiSwagger";
        }
    }
}

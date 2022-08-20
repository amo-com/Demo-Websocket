using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Service.Api.Common
{
    /// <summary>
    /// 自定义路由扩展
    /// </summary>
    public static class RouteExtensions
    {
        public static void UseGeneralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Add(new RoutePrefixConvention(routeAttribute));
        }

        /// <summary>
        /// 使用全局扩展路由前缀
        /// </summary>
        /// <param name="opts">opts</param>
        /// <param name="prefix">prefix</param>
        public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
        {
            opts.UseGeneralRoutePrefix(new RouteAttribute(prefix));
        }

        public class RoutePrefixConvention : IApplicationModelConvention
        {
            private readonly AttributeRouteModel _routePrefix;

            public RoutePrefixConvention(IRouteTemplateProvider route)
            {
                _routePrefix = new AttributeRouteModel(route);
            }

            public void Apply(ApplicationModel application)
            {
                // 遍历所有的 Controller
                foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
                {
                    // 已经标记了 RouteAttribute 的 Controller，在当前路由上再添加一个路由前缀
                    if (selector.AttributeRouteModel != null)
                    {
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                    }

                    // 没有标记RouteAttribute的Controller，添加一个路由前缀
                    else
                    {
                        selector.AttributeRouteModel = _routePrefix;
                    }
                }
            }
        }
    }
}

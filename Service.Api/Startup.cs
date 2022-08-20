using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.Api.Common;
using System;
using System.IO;

namespace Service.Api
{
    public class Startup
    {
        // api endpoint json: /{ApiRoutePrefix}/{apiName}/swagger.json
        private readonly string _apiRoutePrefix;
        private readonly bool _enableShowApiSwagger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _apiRoutePrefix = Configuration.GetValue<string>(ApiCommon.Appsetting.ApiRoutePrefix);
            _enableShowApiSwagger = Configuration.GetValue<bool>(ApiCommon.Appsetting.EnableShowApiSwagger);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_enableShowApiSwagger)
            {
                // 配置swagger 文档
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(ApiCommon.ApiName, new OpenApiInfo { Title = "Api Document V1", Version = "v1" });
                    ApiCommon.XmlDocumentes?.ForEach(x =>
                    {
                        c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x));
                    });

                    // swagger中控制请求的时候发是否需要在url中增加accesstoken
                    // c.OperationFilter<Filters.AddHeaderParameterFilter>();
                    c.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                });
            }

            // 添加全局路由前缀和错误捕捉,配置自定义路由前缀
            // services.AddSingleton<IExceptionFilter, Filters.GlobalExceptionFilter>();
            services.AddMvc(c =>
            {
                c.UseGeneralRoutePrefix(_apiRoutePrefix);

                // c.Filters.Add(typeof(Filters.GlobalExceptionFilter));
                // c.Filters.AddService<IExceptionFilter>();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // 配置跨域支持
            services.AddCors(options => options.AddPolicy(
                ApiCommon.CorsName,
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (_enableShowApiSwagger)
            {
                app.UseSwagger();

                // 注册swagger文档
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = _apiRoutePrefix;
                    c.DocumentTitle = $"{ApiCommon.ApiName} Api Document";

                    c.SwaggerEndpoint($"/{_apiRoutePrefix}/{ApiCommon.ApiName}/swagger.json", "Api Document V1");
                    c.DefaultModelsExpandDepth(-1);
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                });

                // 配置json路径,/api/{documentName}/swagger.json,给上面SwaggerEndpoint使用的
                app.UseSwagger(c => { c.RouteTemplate = $"/{_apiRoutePrefix}/{{documentName}}/swagger.json"; });
            }

            app.UseRouting();
            app.UseCors(ApiCommon.CorsName);

            app.UseAuthentication(); // 使用授权中间件
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

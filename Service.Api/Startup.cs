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
                // ����swagger �ĵ�
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(ApiCommon.ApiName, new OpenApiInfo { Title = "Api Document V1", Version = "v1" });
                    ApiCommon.XmlDocumentes?.ForEach(x =>
                    {
                        c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x));
                    });

                    // swagger�п��������ʱ���Ƿ���Ҫ��url������accesstoken
                    // c.OperationFilter<Filters.AddHeaderParameterFilter>();
                    c.CustomSchemaIds(type => type.FullName); // �����ͬ�����ᱨ�������
                });
            }

            // ���ȫ��·��ǰ׺�ʹ���׽,�����Զ���·��ǰ׺
            // services.AddSingleton<IExceptionFilter, Filters.GlobalExceptionFilter>();
            services.AddMvc(c =>
            {
                c.UseGeneralRoutePrefix(_apiRoutePrefix);

                // c.Filters.Add(typeof(Filters.GlobalExceptionFilter));
                // c.Filters.AddService<IExceptionFilter>();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // ���ÿ���֧��
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

                // ע��swagger�ĵ�
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = _apiRoutePrefix;
                    c.DocumentTitle = $"{ApiCommon.ApiName} Api Document";

                    c.SwaggerEndpoint($"/{_apiRoutePrefix}/{ApiCommon.ApiName}/swagger.json", "Api Document V1");
                    c.DefaultModelsExpandDepth(-1);
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                });

                // ����json·��,/api/{documentName}/swagger.json,������SwaggerEndpointʹ�õ�
                app.UseSwagger(c => { c.RouteTemplate = $"/{_apiRoutePrefix}/{{documentName}}/swagger.json"; });
            }

            app.UseRouting();
            app.UseCors(ApiCommon.CorsName);

            app.UseAuthentication(); // ʹ����Ȩ�м��
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

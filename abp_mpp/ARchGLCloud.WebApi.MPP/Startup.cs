using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ARchGLCloud.Core.Extensions;
using MediatR;

namespace ARchGLCloud.WebApi.MPP
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                // options.AddPrefixedRoute(new RouteAttribute("api/v{version}"), new RouteAttribute("mpp"));
            });

            services.AddMvcCore().AddJsonFormatters().AddAuthorization().AddDataAnnotations();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SPIDERBIM文件管理服务 API v1.0",
                    Description = "FileServer API Swagger surface",
                    Contact = new Contact { Name = "筑智建科技（重庆）有限公司", Email = "023-63880760", Url = "http://www.zzjbim.com" },
                    License = new License { Name = "MIT", Url = "https://github.com/Microsoft/dotnet/blob/master/LICENSE" }
                });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

                var xmlPath = Path.Combine(basePath, "ARchGLCloud.WebApi.MPP.xml");
                s.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            "*"
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            //权限验证
            //services.AddAuthentication("Bearer")
            //   .AddIdentityServerAuthentication(options =>
            //   {
            //       options.Authority = Configuration.GetValue<string>("IdentityURL");
            //       options.RequireHttpsMetadata = false;
            //       options.ApiName = "MPP";
            //   });

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            //services.AddMvc();
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(DefaultCorsPolicyName);
            // app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "SPIDERBIM文件管理服务 API v1.0");
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            BootStrapper.RegisterServices(services);
        }
    }
}

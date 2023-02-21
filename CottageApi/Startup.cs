using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
using CottageApi.Converters;
using CottageApi.Core.Configurations;
using CottageApi.Core.Helpers;
using CottageApi.Data.Context;
using CottageApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace CottageApi
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = GetConfig();

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single();

                jsonInputFormatter.SupportedMediaTypes.Add("text/plain");
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.None;
                    options.SerializerSettings.Converters.Add(new MultiFormatDateConverter
                    {
                        DateTimeFormats = new List<string> { "dd.MM.yyyy HH:mm:ss", "yyyy-MM-ddTHH-mm-ss" }
                    });
                });

            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(config);

            IdentityModelEventSource.ShowPII = true;

            services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            }));

            // register Connections config
            services.AddSingleton(typeof(Config), config);

            services.AddDbContext<CottageDbContext>();

            services.AddScoped<ICottageDbContext>(provider => provider.GetService<CottageDbContext>());

            services.AddOwnDependencies(config);

            LoggingExtensions.ConfigureLogging(config);

            services.AddSwaggerGen();

            services.AddHttpClient("Pivdenniy", c =>
            {
                c.BaseAddress = new Uri(config.PivdenniyBankConfig.APIPaymentUrl);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    AllowAutoRedirect = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CottageDbContext context, Config config)
        {
            // set FilesConfig
            app.Use(async (ctx, next) =>
            {
                var baseDirectory = env.ContentRootPath;
                FilesHelper.SetRootUrl(config.FilesConfig, baseDirectory);

                await next();
            });

            app.UseHsts();
            app.UseHttpsRedirection();

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
            FilesHelper.CheckCreateDirectory(uploadDirectory);

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadDirectory),
                RequestPath = new PathString("/Upload")
            });

            app.UseRouting();
            app.UseCors("AllowAnyOrigin");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseGeneralExceptionMiddleware();

            app.UseRequestLocalization();

            app.UseCustomRequestLocalization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            DbInitializer.Initialize(context);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(config);
        }

        private Config GetConfig()
        {
            var config = new Config();
            Configuration.GetSection("CottageAPI").Bind(config);

            return config;
        }
    }
}

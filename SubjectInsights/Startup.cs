using SubjectInsights.Application.Interfaces.Repositories;
using SubjectInsights.Common;
using SubjectInsights.Infrastructure.Repository;
using SubjectInsights.Service;
using SubjectInsights.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace SubjectInsights.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddRouting(options => options.LowercaseUrls = true).AddOptions();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Subject Insights System", Version = "v1" });
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                  new OpenApiSecurityScheme
                  {
                      Description = "JWT Authorization header using the Bearer scheme.",
                      Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                      Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                  });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });

            });

            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = TokenEngine.GetValidationParameters();
               });
            services.AddSingleton<IServiceApiClient, ServiceApiClient>();
            services.AddScoped<ISubjectInsightsAdminRepository, SubjectInsightsAdminRepository>();
            services.AddScoped<ISubjectInsightsAdminService, SubjectInsightsAdminService>();
            services.AddScoped<ISubjectInsightsUserRepository, SubjectInsightsUserRepository>();
            services.AddScoped<ISubjectInsightsUserService, SubjectInsightsUserService>();
            services.AddTransient<IDbConnection>(_ => new MySqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subject Insights v1"));

            app.UseForwardedHeaders();
            app.UseCors();
            app.UseRouting();
            app.UseHealthChecks("/api/health");
            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

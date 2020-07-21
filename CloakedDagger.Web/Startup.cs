using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloakedDagger.Common;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Data;
using CloakedDagger.Data.Repositories;
using CloakedDagger.Logic.PasswordHasher;
using CloakedDagger.Logic.Services;
using CloakedDagger.Web.Database;
using CloakedDagger.Web.Middleware;
using CloakedDagger.Web.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OwlTin.Common.Converters;
using Serilog;

namespace CloakedDagger.Web
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
            DatabaseMigrator.MigrateDatabase(Configuration.GetDatabaseConnectionString());

            services.AddDbContext<CloakedDaggerDbContext>(options =>
            {
                options.UseMySql(Configuration.GetDatabaseConnectionString());
            });

            services.AddTransient<IUserRepository, UserRepository>();
            
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IPasswordHasher, ArgonPasswordHasher>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LogoutPath = "/user/logout";
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        // Don't want it to redirect to a different URL when not logged in, just return a 401
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Cookie.Name = "CLOAKED_DAGGER_SESSION";
                });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new JsonDateEpochConverter());
                });

            services.AddSingleton(Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
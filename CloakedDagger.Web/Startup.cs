using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Data;
using CloakedDagger.Data.Extensions;
using CloakedDagger.Data.Repositories;
using CloakedDagger.Logic.PasswordHasher;
using CloakedDagger.Logic.Services;
using CloakedDagger.Web.Adapters;
using CloakedDagger.Web.Converters;
using CloakedDagger.Web.Database;
using CloakedDagger.Web.Middleware;
using CloakedDagger.Web.Utils;
using IdentityServer4;
using IdentityServer4.Models;
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
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;
using Client = IdentityServer4.Models.Client;
using Resource = IdentityServer4.Models.Resource;

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
                options.UseMySql(Configuration.GetDatabaseConnectionString(),
                    ServerVersion.AutoDetect(Configuration.GetDatabaseConnectionString()),
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IResourceRepository, ResourceRepository>();
            services.AddTransient<IResourceScopeRepository, ResourceScopeRepository>();
            services.AddTransient<IScopeRepository, ScopeRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IClientEventRepository, ClientEventRepository>();
            
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IResourceScopeService, ResourceScopeService>();

            services.AddTransient<IClientService, ClientService>();

            services.AddTransient<IPasswordHasher, ArgonPasswordHasher>();

            var entityMapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ResourceScopeEntity, ResourceScopeViewModel>()
                    .ForMember(vm => vm.Name, cfg =>
                        cfg.MapFrom(rs => rs.ScopeEntity.Name)
                    )
                    .ForMember(vm => vm.Description, cfg =>
                        cfg.MapFrom(rs => rs.ScopeEntity.Description)
                    );
                config.CreateMap<CloakedDagger.Common.Entities.ResourceEntity, ResourceViewModel>();
            });
            
            var entityMapper = new Mapper(entityMapperConfig);
            services.AddSingleton<IMapper>(entityMapper);
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new JsonDateEpochConverter());
                    options.SerializerSettings.Converters.Add(
                        new MapperJsonConverter<CloakedDagger.Common.Entities.ResourceEntity, ResourceViewModel>(entityMapper));
                    options.SerializerSettings.Converters.Add(new MapperJsonConverter<ResourceScopeEntity, ResourceScopeViewModel>(entityMapper));
                });

            services.AddSingleton(Log.Logger);

            // Identity Server / OAuth2
            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "http://localhost:3333/login";
                })
                .AddClientStore<ClientStoreAdapter>()
                .AddResourceStore<ResourceStoreAdapter>()
                .AddDeveloperSigningCredential();
            
            // Add this after we configure Identity Server, otherwise it overrides the settings or at least the
            //  OnRedirectToLogin handler
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LogoutPath = "/user/logout";
                    options.Cookie.Name = "CLOAKED_DAGGER_SESSION";
                    options.Events.OnRedirectToLogin = context =>
                    {
                        // Don't want it to redirect to a different URL when not logged in, just return a 401
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                });

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
            app.UseIdentityServer();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
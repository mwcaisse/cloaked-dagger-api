using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using CloakedDagger.Common;
using CloakedDagger.Common.Configuration;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Data;
using CloakedDagger.Data.Repositories;
using CloakedDagger.Logic.EmailSender;
using CloakedDagger.Logic.PasswordHasher;
using CloakedDagger.Logic.Services;
using CloakedDagger.Web.Adapters;
using CloakedDagger.Web.Configuration;
using CloakedDagger.Web.Constants;
using CloakedDagger.Web.Converters;
using CloakedDagger.Web.Database;
using CloakedDagger.Web.Middleware;
using CloakedDagger.Web.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OwlTin.Common.Converters;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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
            var authenticationConfiguration = new AuthenticationConfiguration()
            {
                LoginUrl = Configuration.GetValue<string>("authentication:loginUrl"),
                LogoutUrl = Configuration.GetValue<string>("authentication:logoutUrl"),
                CookieName = Configuration.GetValue<string>("authentication:cookieName"),
                Key = Configuration.GetValue<string>("authentication:key"),
                KeyPassword = Configuration.GetValue<string>("authentication:keyPassword")
            };
            services.AddSingleton(authenticationConfiguration);

            var emailConfiguration = new EmailConfiguration()
            {
                FromEmailAddress = Configuration.GetValue<string>("email:fromEmailAddress"),
                FromEmailName = Configuration.GetValue<string>("email:fromEmailName"),
                EmailVerificationUrl = Configuration.GetValue<string>("email:emailVerificationUrl"),
            };
            services.AddSingleton(emailConfiguration);

            var sendGridConfiguration = new SendGridConfiguration()
            {
                ApiKey = Configuration.GetValue<string>("sendGrid:apiKey"),
            };
            services.AddSingleton(sendGridConfiguration);
            
            DatabaseMigrator.MigrateDatabase(Configuration.GetDatabaseConnectionString());

            services.AddDbContext<CloakedDaggerDbContext>(options =>
            {
                options.UseMySql(Configuration.GetDatabaseConnectionString(),
                    ServerVersion.AutoDetect(Configuration.GetDatabaseConnectionString()),
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IUserRegistrationKeyRepository, UserRegistrationKeyRepository>();
            services.AddTransient<IUserRegistrationKeyUseRepository, UserRegistrationKeyUseRepository>();
            services.AddTransient<IUserEmailVerificationRequestRepository, UserEmailVerificationRequestRepository>();
            
            services.AddTransient<IResourceRepository, ResourceRepository>();
            services.AddTransient<IResourceScopeRepository, ResourceScopeRepository>();
            services.AddTransient<IScopeRepository, ScopeRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IClientEventRepository, ClientEventRepository>();
            services.AddTransient<IPersistedGrantRepository, PersistedGrantRepository>();
            
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRegistrationKeyService, UserRegistrationKeyService>();

            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IResourceScopeService, ResourceScopeService>();

            services.AddTransient<IClientService, ClientService>();

            services.AddTransient<IPasswordHasher, ArgonPasswordHasher>();
            
            //Email Stuff
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<IEmailService, EmailService>();

            var entityMapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ResourceScopeEntity, ResourceScopeViewModel>()
                    .ForMember(vm => vm.Name, cfg =>
                        cfg.MapFrom(rs => rs.ScopeEntity.Name)
                    )
                    .ForMember(vm => vm.Description, cfg =>
                        cfg.MapFrom(rs => rs.ScopeEntity.Description)
                    );
                config.CreateMap<ResourceEntity, ResourceViewModel>();
                config.CreateMap<UserRegistrationKeyEntity, UserRegistrationKeyViewModel>();
            });
            
            var entityMapper = new Mapper(entityMapperConfig);
            services.AddSingleton<IMapper>(entityMapper);
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new JsonDateEpochConverter());
                    options.SerializerSettings.Converters.Add(
                        new MapperJsonConverter<ResourceEntity, ResourceViewModel>(entityMapper));
                    options.SerializerSettings.Converters.Add(new MapperJsonConverter<ResourceScopeEntity, ResourceScopeViewModel>(entityMapper));
                    options.SerializerSettings.Converters.Add(new MapperJsonConverter<UserRegistrationKeyEntity, UserRegistrationKeyViewModel>(entityMapper));
                });

            services.AddSingleton(Log.Logger);

            // Identity Server / OAuth2
            var isBuilder = services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = authenticationConfiguration.LoginUrl;
                })
                .AddClientStore<ClientStoreAdapter>()
                .AddResourceStore<ResourceStoreAdapter>()
                .AddPersistedGrantStore<PersistedGrantStoreAdapter>()
                .AddProfileService<ProfileServiceAdapter>();
        

            if (string.IsNullOrWhiteSpace(authenticationConfiguration.Key))
            {
                isBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                isBuilder.AddSigningCredential(LoadAuthenticationSigningKey(authenticationConfiguration));
            }

            // Add this after we configure Identity Server, otherwise it overrides the settings or at least the
            //  OnRedirectToLogin handler
            services.AddAuthentication(CloakedDaggerAuthenticationSchemes.Default)
                .AddCookie(CloakedDaggerAuthenticationSchemes.Default, options =>
                {
                    options.LogoutPath = authenticationConfiguration.LogoutUrl;
                    options.Cookie.Name = authenticationConfiguration.CookieName;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        // Don't want it to redirect to a different URL when not logged in, just return a 401
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                })
                .AddCookie(CloakedDaggerAuthenticationSchemes.Partial, options =>
                {
                    options.LogoutPath = authenticationConfiguration.LogoutUrl;
                    options.Cookie.Name =
                        $"{authenticationConfiguration.CookieName}__{CloakedDaggerAuthenticationSchemes.Partial}";
                    options.Events.OnRedirectToLogin = context =>
                    {
                        // Don't want it to redirect to a different URL when not logged in, just return a 401
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                });
        }

        private X509Certificate2 LoadAuthenticationSigningKey(AuthenticationConfiguration config)
        {
            return new (Convert.FromBase64String(config.Key), config.KeyPassword);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/api", apiApp =>
            {
                apiApp.UseMiddleware<ErrorHandlingMiddleware>();

                // Configure forwarded headers to ensure discovery document of ID Server is in https
                var forwardOptions = new ForwardedHeadersOptions()
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                    RequireHeaderSymmetry = false
                };
                forwardOptions.KnownNetworks.Clear();
                forwardOptions.KnownProxies.Clear();
                app.UseForwardedHeaders(forwardOptions);

                apiApp.UseRouting();

                apiApp.UseAuthentication();
                apiApp.UseAuthorization();
                apiApp.UseIdentityServer();

                apiApp.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            });


        }
    }
}
using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreDataAccess.UnitOfWork;
using NetCoreDI;
using NetCoreIdentity.BusinessLogic;
using NetCoreIdentity.DataAccess;
using Microsoft.AspNetCore.Localization;

namespace NetCoreIdentity
{
    public class Startup
    {
        private const string CertUrlParameter = "AppSettings:CertUrl";
        private const string CertPassParameter = "AppSettings:CertPass";

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services
                //.AddDbContext<NetCoreIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NetCoreIdentityServer")))
                .AddDbContext<NetCoreIdentityDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NetCoreIdentityServer")))
                .AddTransient<DbContext, NetCoreIdentityDbContext>()
                .AddTransient<IExecutor, Executor>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>()
                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
                .AddNetCoreIdentityBusinessLogicDependencies();

            services.AddCors(options =>
            {
                options.AddPolicy("allowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
                options.AddPolicy("localOriginOnly", policy =>
                {
                    policy.WithOrigins(Configuration.GetSection("Identity").GetSection("Url").Value)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddMvc();

            var certUrl = string.IsNullOrEmpty(Configuration[CertUrlParameter])
                ? "wwwroot/Certs/localhost.pfx"
                : Configuration[CertUrlParameter];

            var certPass = string.IsNullOrEmpty(Configuration[CertPassParameter])
                ? "123"
                : Configuration[CertPassParameter];

            services.AddIdentityServer(
                    options =>
                    {
                        options.Authentication.CookieLifetime = TimeSpan.FromHours(24);
                        options.Authentication.CookieSlidingExpiration = false;
                    }
                )
                .AddSigningCredential(new X509Certificate2($"{certUrl}", certPass))
                .AddCustomUserStore()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(Environment.IsProduction()))
                .AddJwtBearerClientAuthentication();

            var serviceProvider = services.BuildServiceProvider();
            var _ = new AmbientContext(serviceProvider);
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
                app.UseExceptionHandler("/Home/Error");
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("kk")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();


            //накатим миграцию при первом пуске приложения            
            using (var db = AmbientContext.Current.Resolver.ResolveObject<NetCoreIdentityDbContext>())
            {
                db.Database.Migrate();
            }
        }
    }
}
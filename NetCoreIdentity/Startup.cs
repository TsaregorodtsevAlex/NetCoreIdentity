using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreDataAccess;
using NetCoreDataAccess.UnitOfWork;
using NetCoreDI;
using NetCoreIdentity.BusinessLogic;
using NetCoreIdentity.DataAccess;
using IdentityServer4.Validation;

namespace NetCoreIdentity
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
            services
                //.AddDbContext<NetCoreIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NetCoreIdentityServer")))
                .AddDbContext<NetCoreIdentityDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("NetCoreIdentityServer")))
                .AddTransient<DbContext, NetCoreIdentityDbContext>()
                .AddTransient<IExecutor, Executor>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>()
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

            services.AddIdentityServer(options =>
                {
                    options.Authentication.CookieLifetime = TimeSpan.FromSeconds(60);
                    options.Authentication.CookieSlidingExpiration = false;
                })
                .AddSigningCredential(new X509Certificate2(@"C:\localhost.pfx", "123"))
                //.AddDeveloperSigningCredential()
                .AddCustomUserStore()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                //.AddRedirectUriValidator<IRedirectUriValidator>()
                .AddJwtBearerClientAuthentication();
                //.AddCorsPolicyService<CorsPolicyService>()
                //.AddProfileService<ProfileService>();

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

            app.UseCors(builder =>
                builder.WithOrigins("https://localhost:44315").AllowAnyHeader());

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}

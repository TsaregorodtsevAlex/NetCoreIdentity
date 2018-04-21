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
                .AddDbContext<NetCoreIdentityDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NetCoreIdentityServer")))
                .AddTransient<DbContext, NetCoreIdentityDbContext>()
                .AddTransient<IExecutor, Executor>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>()
                .AddNetCoreIdentityBusinessLogicQueries()
                .AddNetCoreIdentityBusinessLogicCommands();


            services.AddMvc();

            services.AddIdentityServer()
                    .AddCustomUserStore()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryClients(Config.GetClients());

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

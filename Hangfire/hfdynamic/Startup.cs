using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hfdynamic
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
            services.AddControllers();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseDefaultTypeResolver()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection")));
            AppDomain.CurrentDomain.AssemblyResolve += 
                new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            services.AddHangfireServer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new AllowAllConnectionsFilter() },
                IgnoreAntiforgeryToken = true
            });

            app.UseHangfireServer();
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private const string pluginFolder = "plugins";
        static Assembly CurrentDomain_AssemblyResolve(object sender,
                                  ResolveEventArgs args)
        {
            var assemblyname = new AssemblyName(args.Name).Name;
            Console.WriteLine(assemblyname);

            DynamicLoad load = new DynamicLoad();
            var assembly = load.Load($"plugin1/{assemblyname}.dll");

/*             var assemblyname = new AssemblyName(args.Name).Name;
            var assemblyFileName = Path.Combine("plugins", "plugin1", assemblyname + ".dll");
            var assembly = Assembly.LoadFrom(assemblyFileName); */

            return assembly.Item2;
        }
        public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // Allow outside. You need an authentication scenario for this part.
                // DON'T GO PRODUCTION WITH THIS LINES.
                return true;
            }
        }
    }
}

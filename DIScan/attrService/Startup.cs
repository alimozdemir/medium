using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace attrService
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
            /* Solution 1 */
            /* 
            services.Scan(i => 
                i.FromCallingAssembly()
                .AddClasses(c => c.AssignableTo<ITransient>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                
                .AddClasses(c => c.AssignableTo<IScoped>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                
                .AddClasses(c => c.AssignableTo<ISingleton>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                );
            */

            
            /* Solution 2 */
            /* services.Scan(i => 
                i.FromCallingAssembly()
                .AddClasses(c => c.WithAttribute<TransientAttribute>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                
                .AddClasses(c => c.WithAttribute<ScopedAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                
                .AddClasses(c => c.WithAttribute<SingletonAttribute>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
            );
            */

            /* Solution 3 */
            services.Scan(i => 
                i.FromCallingAssembly()
                .InjectableAttributes()
            );

            /* Or */
            /* services.Scan(i => 
                i.FromCallingAssembly()
                .InjectableAttribute(ServiceLifetime.Transient)
                .InjectableAttribute(ServiceLifetime.Scoped)
                .InjectableAttribute(ServiceLifetime.Singleton)
            ); */

            /* Business Example */

            /*
            services.Scan(i => 
                i.FromCallingAssembly()
                .AddClasses(c => c.Where(i => i.IsClass && i.Name.EndsWith("BusinessService")))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
            */
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

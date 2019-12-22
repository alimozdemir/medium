using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hfdynamic
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var test = new DynamicLoad();
            var t = test.GetType();
            Console.WriteLine(t.AssemblyQualifiedName);

            var s = "hfdynamic.DynamicLoad2, hfdynamic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            var t2 = Type.GetType(s, false);


            if (t2 == null)
            {
                Console.WriteLine("Not found");
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

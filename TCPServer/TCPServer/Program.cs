using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPServer.Configuration;
using TCPServer.Services;

namespace TCPServer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // create service collection
            var services = new ServiceCollection();
            ConfigureServices(services);

            // create service provider
            var serviceProvider = services.BuildServiceProvider();

            // entry to run app
            
            await serviceProvider.GetService<App>().Run(args);


            

        }

        
        private static void ConfigureServices(IServiceCollection services)
        {
            // configure logging
            services.AddLogging(builder =>
            {
                //builder.AddConsole();
                //builder.AddDebug();
            });



            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                //.AddEnvironmentVariables()
                .Build();

            services.Configure<TenantConfiguration>(configuration.GetSection("TenantConfiguration"));

            // add services:
            services.AddSingleton<ITenantConfiguration>(sp =>
                (ITenantConfiguration)sp.GetRequiredService<IOptions<TenantConfiguration>>().Value);
            services.AddTransient<IDevicePlayService, DevicePlayService>();

            // add app
            services.AddTransient<App>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace CallawayPreOwnedService
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)                
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(hostContext.Configuration);
                    services.AddHostedService<Worker>();
                    services.AddHttpClient<Services.CallawayPreOwnedService>(c => c.BaseAddress = new Uri(Services.CallawayPreOwnedService.API_BASE_ADDRESS));
                    
                    // Wanted Products
                    services.AddSingleton<List<Models.Product>>(c => new List<Models.Product>() { 
                        new Models.Product() { Club = "4 Iron" },
                    });

                    services.AddSingleton<Services.ProductAlertService>();
                });
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    
                    // Wanted Products (loaded dynamically from appsettings)
                    services.AddSingleton<List<Models.Product>>(c => hostContext.Configuration.GetSection(Models.WantedProductOptions.Key).Get<Models.WantedProductOptions>().WantedProductsList.ToList());
                    services.AddSingleton<Services.ProductAlertService>();
                });
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CallawayPreOwnedService.Models;
using Microsoft.Extensions.Configuration;

namespace CallawayPreOwnedService
{
    public class Worker : BackgroundService
    {
        private const int WORKER_INTERVAL_IN_MINUTES = 60;
        private readonly IConfiguration _config;
        private readonly ILogger<Worker> _logger;
        private readonly Services.CallawayPreOwnedService _callawayPreOwnedService;
        private readonly Services.ProductAlertService _prodAlertService;

        public Worker(ILogger<Worker> logger, Services.CallawayPreOwnedService callawayPreOwnedService, Services.ProductAlertService prodAlertService, IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _callawayPreOwnedService = callawayPreOwnedService;
            _prodAlertService = prodAlertService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var prodSearchOptions = _config.GetSection(ProductSearchOptions.Key).Get<ProductSearchOptions>();
                if(prodSearchOptions != null)
                {
                    var productsAvailable = GetAvailableProducts(prodSearchOptions.ProductSearchOptionsList);
                    var productsWanted = _prodAlertService.CheckForWantedProducts(productsAvailable);

                    if(productsWanted.Count > 0)
                    {
                        _logger.LogInformation("Sending out email alert for wanted products.");
                        var success = _prodAlertService.SendOutProductAlert(productsWanted);
                        if(success)
                        {
                            _logger.LogInformation("E-mail alert sent out successfully.");
                        }
                        else
                        {
                            _logger.LogError("E-mail send failed.");
                        }
                    }
                }
                else
                {
                    _logger.LogError("Unable to load Product Search Options. Please ensure the ProductSearchOptions section is properly configured in appsettings.json");
                }

                await Task.Delay(WORKER_INTERVAL_IN_MINUTES * 60 * 1000, stoppingToken);
            }
        }        
        private List<Product> GetAvailableProducts(ProductSearchOption[] productSearchOptions)
        {            
            var availableProducts = new List<Product>();

            foreach(var prodSearchOption in productSearchOptions)
            {
                ProductVariantData productData = _callawayPreOwnedService.GetProductVariantData(prodSearchOption.ProductID, prodSearchOption.CGID, prodSearchOption.GenderHand);
                if(productData != null)
                {
                    _logger.LogInformation("Successfully deserialized Product Data:" + Environment.NewLine + productData.ToString());
                    availableProducts.AddRange(productData.CreateProducts());
                }
                else
                {
                    _logger.LogInformation("Product Data is null");
                }
            }
            return availableProducts;
        }
    }
}

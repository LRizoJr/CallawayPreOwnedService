using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CallawayPreOwnedService.Models;

namespace CallawayPreOwnedService
{
    public class Worker : BackgroundService
    {
        private const int WORKER_INTERVAL_IN_MINUTES = 60;
        private readonly ILogger<Worker> _logger;
        private readonly Services.CallawayPreOwnedService _callawayPreOwnedService;
        private readonly Services.ProductAlertService _prodAlertService;

        public Worker(ILogger<Worker> logger, Services.CallawayPreOwnedService callawayPreOwnedService, Services.ProductAlertService prodAlertService)
        {
            _logger = logger;
            _callawayPreOwnedService = callawayPreOwnedService;
            _prodAlertService = prodAlertService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                var productsAvailable = GetAvailableProducts(new MensRightApexIronsParams());
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

                await Task.Delay(WORKER_INTERVAL_IN_MINUTES * 60 * 1000, stoppingToken);
            }
        }        
        private List<Product> GetAvailableProducts(GetProductParams productParams)
        {            
            var availableProducts = new List<Product>();

            ProductVariantData productData = _callawayPreOwnedService.GetProductVariantData(productParams.ProductID, productParams.CGID, productParams.GenderHand);
            
            if(productData != null)
            {
                _logger.LogInformation("Successfully deserialized Product Data:" + Environment.NewLine + productData.ToString());
                availableProducts.AddRange(productData.CreateProducts());
            }
            else
            {
                _logger.LogInformation("Product Data is null");
            }
            return availableProducts;
        }
    }
}

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CallawayPreOwnedService.Models;
using System.Linq;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CallawayPreOwnedService.Services
{
    public class ProductAlertService
    {
        private readonly IConfiguration _config;        
        private readonly List<Product> _targetProducts;
        private readonly ILogger<Worker> _logger;
        public ProductAlertService(ILogger<Worker> logger, List<Product> targetProducts, IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _targetProducts = targetProducts;
        }

        public List<Product> CheckForWantedProducts(List<Product> availableProducts)
        {
            var wantedProducts = new List<Product>();

            foreach(var availableProduct in availableProducts)
            {
                // Check if this is something we're looking for
                if(_targetProducts.Where(target =>  (string.IsNullOrEmpty(target.ParentProductID) || target.ParentProductID.Equals(availableProduct.ParentProductID, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.Club) || target.Club.Equals(availableProduct.Club, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.ShaftMaterial) || target.ShaftMaterial.Equals(availableProduct.ShaftMaterial, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.ShaftFlex) || target.ShaftFlex.Equals(availableProduct.ShaftFlex, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.ShaftType) || target.ShaftType.Equals(availableProduct.ShaftType, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.LieAngle) || target.LieAngle.Equals(availableProduct.LieAngle, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.Length) || target.Length.Equals(availableProduct.Length, StringComparison.OrdinalIgnoreCase))
                                                    && (string.IsNullOrEmpty(target.Condition) || target.Condition.Equals(availableProduct.Condition, StringComparison.OrdinalIgnoreCase))).Count() > 0)
                {
                    wantedProducts.Add(availableProduct);
                }
            }

            _logger.LogInformation($"Found {wantedProducts.Count} product(s) matching wanted criteria.");
            foreach(var wantedProduct in wantedProducts)
            {
                _logger.LogInformation("Product:" + Environment.NewLine + wantedProduct.ToString());
            }
            return wantedProducts;
        }

        public bool SendOutProductAlert(List<Product> wantedProducts)
        {
            if(wantedProducts.Count > 0)
            {
                var sendGridAPIKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var emailOptions = _config.GetSection(EmailOptions.Key).Get<EmailOptions>();
                if(string.IsNullOrEmpty(sendGridAPIKey))
                {
                    _logger.LogError("Unable to find SENDGRID_API_KEY Environment variable. Cannot send email.");
                    return false;
                }
                else if(emailOptions == null)
                {
                    _logger.LogError("Unable to load EmailOptions from appsettings. Please ensure that the EmailOptions section exists.");
                    return false;
                }
                else
                {
                    var sendGridClient = new SendGridClient(sendGridAPIKey);
                    var from = new EmailAddress(emailOptions.EmailFrom, "Callway Pre-Owned Service");
                    var to = new EmailAddress(emailOptions.EmailTo, emailOptions.EmailToName);
                    var subject = "Callaway Pre-Owned Service Alert: Wanted Product(s) Available";                        

                    StringBuilder htmlContent = new StringBuilder($@"<h2>{wantedProducts.Count} Wanted Product(s) Available:<h2/>");                
                    htmlContent.AppendLine("<div><ul>");
                    foreach(var wantedProduct in wantedProducts)
                    {
                        htmlContent.AppendLine($"<li>{wantedProduct.ToHtmlString()}</li>");
                    }
                    htmlContent.AppendLine("</ul></div>");

                    var message = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, to, subject,htmlContent.ToString(), htmlContent.ToString());
                    
                    var response = sendGridClient.SendEmailAsync(message).Result;
                    
                    if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        return true;
                    }
                    else
                    {
                        _logger.LogError($"SendGrid.SendEmail returned an unexpected response:{Environment.NewLine}{response.StatusCode} - {response.Body.ReadAsStringAsync().Result}");
                        return false;
                    }
                }
            }
            else
            {
                _logger.LogWarning("SendOutProductAlert was called even though there are no wanted products available.");
                return true;
            }             
        }
    }
}
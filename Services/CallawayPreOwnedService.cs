using System;
using Microsoft.Extensions.Logging;
using CallawayPreOwnedService.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace CallawayPreOwnedService.Services
{
    public class CallawayPreOwnedService
    {
        public const string API_BASE_ADDRESS = "https://www.callawaygolfpreowned.com/on/demandware.store/Sites-CGPO5-Site/default/";
        public const string API_ENDPOINT_PRODUCT_VARIANT_DATA = "Product-VariantData?";
        private const string API_ENDPOINT_PRODUCT_VARIANT_DATA_PARAMS = "pid={0}&cgid={1}&format=json&genderHand={2}";
        private readonly HttpClient _client;
        private readonly ILogger<Worker> _logger;

        public CallawayPreOwnedService(HttpClient client, ILogger<Worker> logger)
        {
            _client = client;
            _logger = logger;
        }
        public ProductVariantData GetProductVariantData(string pid, string cgid, string genderHand)
        {    
            ProductVariantData dataResponse = null;        
            string endpoint = API_ENDPOINT_PRODUCT_VARIANT_DATA + string.Format(API_ENDPOINT_PRODUCT_VARIANT_DATA_PARAMS, pid, cgid, genderHand);

            _logger.LogInformation($"Calling CallawayPreOwnedService.GetProductVariantData at endpoint: {endpoint}");
            var response = _client.GetAsync(endpoint).Result;
            if(response.IsSuccessStatusCode)
            {
                try
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    _logger.LogInformation("Response string: " + responseString);
                    dataResponse = JsonConvert.DeserializeObject<ProductVariantData>(responseString);
                }
                catch(Exception ex)
                {
                    _logger.LogError("Exception caught while trying to Deserialize GetProductVariantData response." + Environment.NewLine + ex);                    
                }                
            }
            else
            {
                _logger.LogWarning($"Unsuccessful response from GetProductVariantData: {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
            }
            return dataResponse;            
        }
    }
}
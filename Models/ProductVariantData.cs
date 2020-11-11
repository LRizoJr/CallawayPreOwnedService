using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CallawayPreOwnedService.Models
{
    public class ProductVariantData
    {
        public string pid { get; set; }
        public int start { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
                
        public List<List<Attribute>> variants { get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public List<Product> CreateProducts()
        {            
            var products = new List<Product>();
            
            if(variants.Count > 0)
            {       
                // Each variant is a product                         
                foreach(var variant in variants)
                {
                    var product = new Product();
                    // List of Product Attributes (eg: Club, Shaft Material, Length, etc.)
                    foreach(var attribute in variant)
                    {
                        product.ParentProductID = pid;
                        if(attribute != null)
                        {
                            switch(attribute.label)
                            {
                                case "Club":
                                case "Club/Set":
                                    product.Club = attribute.value;
                                    break;                            
                                case "Shaft Material":
                                    product.ShaftMaterial = attribute.value;
                                    break;
                                case "Shaft Flex":
                                    product.ShaftFlex = attribute.value;
                                    break;
                                case "Shaft Type":
                                    product.ShaftType = attribute.value;
                                    break;
                                case "Lie Angle":
                                    product.LieAngle = attribute.value;
                                    break;
                                case "Length":
                                    product.Length = attribute.value;
                                    break;
                                default:
                                    // This could be the condition object or some other attribute we don't know about
                                    if(Product.Conditions.Contains(attribute.label))
                                    {
                                        product.Condition = attribute.label;

                                        // It's a condition object. Value should be an array with: [ ItemNo, ActualPrice, RetailPrice, ProductURI, InStockFlag ]
                                        JArray conditionValues = attribute.value;
                                        if(conditionValues != null)
                                        {
                                            for(int i = 0; i < conditionValues.Count; i++)
                                            {                                            
                                                switch(i)
                                                {
                                                    case 0:
                                                        product.ItemNo = conditionValues[i].ToString();
                                                        break;
                                                    case 1:
                                                        product.ActualPrice = conditionValues[i].ToString();
                                                        break;
                                                    case 2:
                                                        product.RetailPrice = conditionValues[i].ToString();
                                                        break;
                                                    case 3:
                                                        product.ProductURI = conditionValues[i].ToString();
                                                        break;
                                                    case 4:
                                                        product.InStock = conditionValues[i].ToString();
                                                        break;                                                    
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    products.Add(product);                    
                }
            }
            return products;
        }
    }

    public class Attribute
    {
        public string label {get; set;}
        public dynamic value { get; set; }  // this can be a string or an array of strings (in the case of the "Average" label)
    }
}
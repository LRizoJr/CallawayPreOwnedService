using System;
using System.Collections.Generic;

namespace CallawayPreOwnedService.Models
{ 
    public class Product
    {
        public static List<string> Conditions = new List<string> { "Like New", "Very Good", "Good", "Average" };
        public string ParentProductID { get; set; }
        public string Club { get; set; }
        public string ShaftMaterial { get; set; }
        public string ShaftFlex { get; set; }
        public string ShaftType { get; set; }
        public string LieAngle { get; set; }
        public string Length { get; set; }
        public string Condition { get; set; }
        public string ItemNo { get; set; }
        public string RetailPrice {get; set; }
        public string ActualPrice { get; set; }
        public string ProductURI { get; set; }        
        public string InStock { get; set;}        // Not sure what this last boolean flag means, assuming it indicates item is in stock as it's always "true" in the response

        public override string ToString()
        {
            return 
            $@"Parent Product ID: {this.ParentProductID}
                Club: {this.Club}
                Shaft Material: {this.ShaftMaterial}
                Shaft Flex: {this.ShaftFlex}
                Shaft Type: {this.ShaftType}
                Lie Angle: {this.LieAngle}
                Length: {this.Length}
                Condition: {this.Condition}
                Item No: {this.ItemNo}
                Retail Price: {this.RetailPrice}
                Actual Price: {this.ActualPrice}
                Product URI: {this.ProductURI}
                In Stock: {this.InStock}";
        }

        public string ToHtmlString()
        {
            return ToString().Replace(Environment.NewLine, "<br/>");
        }
    }
}
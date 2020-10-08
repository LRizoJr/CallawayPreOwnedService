namespace CallawayPreOwnedService.Models
{
    public class ProductSearchOptions : IOptions
    {
        public static string Key { get; } = "ProductSearchOptions";

        public ProductSearchOption[] ProductSearchOptionsList {get; set; }
    }

    public class ProductSearchOption
    {
        public string ProductID {  get; set; }
        public string CGID { get; set; }
        public string GenderHand { get; set; }
    }
    
}
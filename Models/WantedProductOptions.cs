namespace CallawayPreOwnedService.Models
{
    public class WantedProductOptions : IOptions
    {
        public static string Key { get; } = "WantedProductOptions";

        public Product[] WantedProductsList { get; set; }
    }
}
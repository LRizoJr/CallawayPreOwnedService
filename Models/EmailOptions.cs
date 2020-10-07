namespace CallawayPreOwnedService.Models
{
    public class EmailOptions : IOptions
    {
        public static string Key { get; } = "EmailOptions";

        public string EmailFrom { get; set; }
        public string EmailTo { get; set;}
        public string EmailToName { get; set; }
    }
}
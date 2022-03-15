namespace Contacting.Domain.Shared
{
    public class AppSettings
    {
        public string ServiceName { get; set; }
        public string FrontendAuctionUrl { get; set; }
        public string AuctionStoragePath { get; set; }
        public string ApplicationUrl { get; set; }
        public string EventBusConnection { get; set; }
        public bool UseCustomizationData { get; set; }
        public bool AzureStorageEnabled { get; set; }
    }
}

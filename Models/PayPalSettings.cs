namespace MusicStore.Models
{
    public class PayPalSettings
    {
        public const string PayPalCredentials = "PayPal";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}

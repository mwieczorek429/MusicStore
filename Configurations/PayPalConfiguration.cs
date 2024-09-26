using Microsoft.Extensions.Options;
using MusicStore.Models;
using PayPal.Api;

namespace MusicStore.Configurations
{
    public class PayPalConfiguration
    {
        private readonly PayPalSettings _payPalSettings;

        public PayPalConfiguration(IOptions<PayPalSettings> payPalSettings)
        {
            _payPalSettings = payPalSettings.Value;
        }

        public APIContext GetAPIContext()
        {
            var config = GetConfig();
            var accessToken = new OAuthTokenCredential(_payPalSettings.ClientId, _payPalSettings.ClientSecret, config).GetAccessToken();
            return new APIContext(accessToken) { Config = config };
        }

        private static Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>()
            {
                { "mode", "sandbox" } 
            };
        }
    }
}

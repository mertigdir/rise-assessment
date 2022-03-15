using System;

namespace Contacting.Domain.Shared
{
    public static class SettingOperations
    {
        private static AppSettings _settings;

        public static void Initializer(AppSettings settings)
        {
            _settings = settings;
        }

        public static string GetFrontendAuctionUrl()
        {
            return _settings.FrontendAuctionUrl;
        }
    }
}

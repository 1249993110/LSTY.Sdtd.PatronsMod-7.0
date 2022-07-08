using System.Configuration;
using System.Reflection;

namespace LSTY.Sdtd.PatronsMod
{
    internal static class AppSettings
    {
        public static string AccessToken { get; } = GetSetting(nameof(AccessToken));

        public static string SignalRUrl { get; } = GetSetting(nameof(SignalRUrl));

        private static string GetSetting(string key)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings;
            return config[key]?.Value;
        }
    }
}
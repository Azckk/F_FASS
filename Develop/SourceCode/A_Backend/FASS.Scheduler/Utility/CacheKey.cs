namespace FASS.Scheduler.Utility
{
    public static class CacheKey
    {
        public static class Login
        {
            public static string Captcha => $"LoginCaptcha{Guid.NewGuid()}";
        }
        public static class Setting
        {
            public static string Config => "SettingConfig";
            public static string DictItem => "SettingDictItem";
            public static string ConfigData => "SettingConfigData";
            public static string ConfigService => "SettingConfigService";
        }
        public static class Dashboard
        {
            public static string Home(string username) => $"DashboardHome{username}";
        }
    }
}

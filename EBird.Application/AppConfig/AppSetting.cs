namespace EBird.Application.AppConfig
{
    public class AppSetting
    {
        public static string AppSettingString = "AppSettings";

        public string FirebaseConfigPath { get; set; }
        public string GoogleCloudStorageBucket { get; set; }
    }
}
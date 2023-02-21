namespace CottageApi.Core.Configurations
{
    public class Config
    {
        public ConnectionsConfig Connections { get; set; }

        public IdentityConfig Identity { get; set; }

        public string[] ClientOrigins { get; set; }

        public FilesConfig FilesConfig { get; set; }

        public decimal CottageVillageArea { get; set; }

        public OneSignalConfig OneSignalConfig { get; set; }

        public string[] CameraIPs { get; set; }

        public PivdenniyBankConfig PivdenniyBankConfig { get; set; }

        public string TelegramBotApiKey { get; set; }
    }
}
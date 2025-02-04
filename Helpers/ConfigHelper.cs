using DotNetEnv;
using System.Text;

namespace WindowsApp.Helpers
{
    public sealed class ConfigHelper
    {
        private static readonly Lazy<ConfigHelper> _instance = new(() => new ConfigHelper());
        private readonly APPConfig _config;

        private ConfigHelper()
        {
            // Carregar o .env
            Env.Load();

            _config = new APPConfig
            {
                DefaultPathForProjects = Env.GetString("DEFAULT_PATH_FOR_PROJECTS"),
                MetaDataPath = Env.GetString("METADATA_PATH"),
                Development = Env.GetBool("DEVELOPMENT"),
                SyncInterval = Env.GetInt("SYNC_INTERVAL"),
                APIConfigs = new APIConfigs
                {
                    Token = Env.GetString("TOKEN"),
                    ClientId = Env.GetString("CLIENT_ID"),
                    ClientSecret = Env.GetString("CLIENT_SECRET"),
                    EnterpriseId = Env.GetString("ENTERPRISE_ID"),
                    JwtPrivateKey = Env.GetString("JWT_PRIVATE_KEY"),
                    JwtPrivateKeyPassword = Env.GetString("JWT_PRIVATE_KEY_PASSWORD"),
                    JwtPublicKeyId = Env.GetString("JWT_PUBLIC_KEY_ID"),
                    UserID = Env.GetString("USER_ID"),
                    AZURE_STORAGE_CONNECTION_STRING = Env.GetString("AZURE_STORAGE_CONNECTION_STRING")
                },
                FirebaseConfig = new FirebaseConfig
                {
                    Type = Env.GetString("FIREBASE_TYPE"),
                    ProjectId = Env.GetString("FIREBASE_PROJECT_ID"),
                    PrivateKeyId = Env.GetString("FIREBASE_PRIVATE_KEY_ID"),
                    PrivateKey = Env.GetString("FIREBASE_PRIVATE_KEY"),
                    ClientEmail = Env.GetString("FIREBASE_CLIENT_EMAIL"),
                    ClientId = Env.GetString("FIREBASE_CLIENT_ID"),
                    AuthUri = Env.GetString("FIREBASE_AUTH_URI"),
                    TokenUri = Env.GetString("FIREBASE_TOKEN_URI"),
                    AuthProviderX509CertUrl = Env.GetString("FIREBASE_AUTH_PROVIDER_X509_CERT_URL"),
                    ClientX509CertUrl = Env.GetString("FIREBASE_CLIENT_X509_CERT_URL"),
                    UniverseDomain = Env.GetString("FIREBASE_UNIVERSE_DOMAIN")
                },
                FirebaseAppID = Env.GetString("FIREBASE_APP_ID")
            };
        }

        public static ConfigHelper Instance => _instance.Value;

        public APPConfig GetConfig() => _config;

        public T GetValue<T>(Func<APPConfig, T> selector) => selector(_config);
    }

        public class ModifyAppSetting
        {
            public static async Task<bool> ChangeAppSettings(ChangeSettings settings)
            {
                var configHelper = ConfigHelper.Instance;
                var _config = configHelper.GetConfig();

                // Atualiza os valores conforme as configurações fornecidas
                if (!string.IsNullOrEmpty(settings.DefaultPathForProjects))
                {
                    _config.DefaultPathForProjects = settings.DefaultPathForProjects;
                }

                _config.Development = settings.Development;
                _config.SyncInterval = settings.SyncInterval;

                if (!string.IsNullOrEmpty(settings.Token))
                {
                    _config.APIConfigs.Token = settings.Token;
                }

                // Salva as alterações no `.env`
                SaveConfigToEnv(_config);

                return await Task.FromResult(true); // Retorna true para indicar sucesso
            }

            private static void SaveConfigToEnv(APPConfig config)
            {
                StringBuilder envContent = new StringBuilder();

                envContent.AppendLine($"DEFAULT_PATH_FOR_PROJECTS={config.DefaultPathForProjects}");
                envContent.AppendLine($"METADATA_PATH={config.MetaDataPath}");
                envContent.AppendLine($"DEVELOPMENT={config.Development.ToString().ToLower()}");
                envContent.AppendLine($"SYNC_INTERVAL={config.SyncInterval}");

                envContent.AppendLine($"TOKEN={config.APIConfigs.Token}");
                envContent.AppendLine($"CLIENT_ID={config.APIConfigs.ClientId}");
                envContent.AppendLine($"CLIENT_SECRET={config.APIConfigs.ClientSecret}");
                envContent.AppendLine($"ENTERPRISE_ID={config.APIConfigs.EnterpriseId}");

                envContent.AppendLine($"JWT_PRIVATE_KEY=\"{config.APIConfigs.JwtPrivateKey}\"");
                envContent.AppendLine($"JWT_PRIVATE_KEY_PASSWORD={config.APIConfigs.JwtPrivateKeyPassword}");
                envContent.AppendLine($"JWT_PUBLIC_KEY_ID={config.APIConfigs.JwtPublicKeyId}");
                envContent.AppendLine($"USER_ID={config.APIConfigs.UserID}");
                envContent.AppendLine($"AZURE_STORAGE_CONNECTION_STRING=\"{config.APIConfigs.AZURE_STORAGE_CONNECTION_STRING}\"");

                envContent.AppendLine($"FIREBASE_APP_ID={config.FirebaseAppID}");

                envContent.AppendLine($"FIREBASE_TYPE={config.FirebaseConfig.Type}");
                envContent.AppendLine($"FIREBASE_PROJECT_ID={config.FirebaseConfig.ProjectId}");
                envContent.AppendLine($"FIREBASE_PRIVATE_KEY_ID={config.FirebaseConfig.PrivateKeyId}");
                envContent.AppendLine($"FIREBASE_PRIVATE_KEY=\"{config.FirebaseConfig.PrivateKey}\"");
                envContent.AppendLine($"FIREBASE_CLIENT_EMAIL={config.FirebaseConfig.ClientEmail}");
                envContent.AppendLine($"FIREBASE_CLIENT_ID={config.FirebaseConfig.ClientId}");
                envContent.AppendLine($"FIREBASE_AUTH_URI={config.FirebaseConfig.AuthUri}");
                envContent.AppendLine($"FIREBASE_TOKEN_URI={config.FirebaseConfig.TokenUri}");
                envContent.AppendLine($"FIREBASE_AUTH_PROVIDER_X509_CERT_URL={config.FirebaseConfig.AuthProviderX509CertUrl}");
                envContent.AppendLine($"FIREBASE_CLIENT_X509_CERT_URL={config.FirebaseConfig.ClientX509CertUrl}");
                envContent.AppendLine($"FIREBASE_UNIVERSE_DOMAIN={config.FirebaseConfig.UniverseDomain}");

                // Sobrescreve o arquivo `.env` com as novas configurações
                File.WriteAllText(".env", envContent.ToString());
            }
        }

        public class ChangeSettings
        {
            public required string Token { get; set; }
            public required string DefaultPathForProjects { get; set; }
            public required bool Development { get; set; }
            public int SyncInterval { get; set; }
        }

    public class APPConfig
    {
        public required string DefaultPathForProjects { get; set; }
        public required string MetaDataPath { get; set; }
        public required bool Development { get; set; }
        public int SyncInterval { get; set; }
        public required APIConfigs APIConfigs { get; set; }
        public required FirebaseConfig FirebaseConfig { get; set; }
        public required string FirebaseAppID { get; set; }
    }

    public class FirebaseConfig
    {
        public required string Type { get; set; }
        public required string ProjectId { get; set; }
        public required string PrivateKeyId { get; set; }
        public required string PrivateKey { get; set; }
        public required string ClientEmail { get; set; }
        public required string ClientId { get; set; }
        public required string AuthUri { get; set; }
        public required string TokenUri { get; set; }
        public required string AuthProviderX509CertUrl { get; set; }
        public required string ClientX509CertUrl { get; set; }
        public required string UniverseDomain { get; set; }
    }
    public class APIConfigs
    {
        public required string Token { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string EnterpriseId { get; set; }
        public required string JwtPrivateKey { get; set; }
        public required string JwtPrivateKeyPassword { get; set; }
        public required string JwtPublicKeyId { get; set; }
        public required string UserID { get; set; }
        public required string AZURE_STORAGE_CONNECTION_STRING { get; set; }
    }
}

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using WindowsApp.Helpers;

/*
    Funciona apenas com: 
    $env:GOOGLE_APPLICATION_CREDENTIALS="D:\SyncProfileProjects\Projects\Sync_SyncProfileProjects\WindowsApp\Keys\serviceAccountKey.json"  
*/

namespace WindowsAppSync.Services.API{
   public class FirebaseAuthenticator
    {
        private static FirebaseApp? _firebaseApp;
         public static void AuthenticateWithOAuthAsync()
        {
            try
            {
                var _config = ConfigHelper.Instance.GetConfig();
                var firebaseConfig = _config.FirebaseConfig;

                // Criando um objeto de credenciais diretamente com os dados do `.env`
                var googleCredential = GoogleCredential.FromJson($@"
                {{
                    ""type"": ""{firebaseConfig.Type}"",
                    ""project_id"": ""{firebaseConfig.ProjectId}"",
                    ""private_key_id"": ""{firebaseConfig.PrivateKeyId}"",
                    ""private_key"": ""{firebaseConfig.PrivateKey.Replace("\n", "\\n")}"",
                    ""client_email"": ""{firebaseConfig.ClientEmail}"",
                    ""client_id"": ""{firebaseConfig.ClientId}"",
                    ""auth_uri"": ""{firebaseConfig.AuthUri}"",
                    ""token_uri"": ""{firebaseConfig.TokenUri}"",
                    ""auth_provider_x509_cert_url"": ""{firebaseConfig.AuthProviderX509CertUrl}"",
                    ""client_x509_cert_url"": ""{firebaseConfig.ClientX509CertUrl}"",
                    ""universe_domain"": ""{firebaseConfig.UniverseDomain}""
                }}");

                // Inicializar o Firebase Admin SDK com as credenciais
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = googleCredential
                });

                Console.WriteLine("Firebase Admin SDK inicializado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar Firebase Admin SDK: {ex.Message}");
            }
        }

         // Método para obter uma instância do FirestoreDB
        public static FirestoreDb GetFirestoreDb()
        {
            var _config = ConfigHelper.Instance.GetConfig();
            if (_firebaseApp == null)
            {
                throw new Exception("Erro: FirebaseAuth not defined");
            }
            // Retorna a instância do FirestoreDb associada à aplicação
            return FirestoreDb.Create(_config.FirebaseAppID);
        }

        public static void SourceFirebaseKeys(){
             // Obter o diretório atual da aplicação (onde o executável está localizado)
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construir o caminho dinâmico para o arquivo de credenciais
            string credentialsPath = System.IO.Path.Combine(appDirectory, "Keys", "serviceAccountKey.json");

            // Verificar se o arquivo existe antes de definir a variável de ambiente
            if (File.Exists(credentialsPath))
            {
                // Definir a variável de ambiente com o caminho absoluto do arquivo de credenciais
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

                // Aqui você pode continuar o fluxo de autenticação do Firebase
                Console.WriteLine("Variável de ambiente GOOGLE_APPLICATION_CREDENTIALS definida com sucesso.");
            }
            else
            {
                Console.WriteLine("Arquivo de credenciais não encontrado.");
            }
        }
    }

}
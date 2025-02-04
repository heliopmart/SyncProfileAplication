using Azure.Storage.Blobs;
using WindowsApp.Helpers;

namespace WindowsApp.Services.API{
    public class AzureAuthenticator
{
    private readonly string _connectionString;
    private BlobServiceClient _blobServiceClient;

    public AzureAuthenticator()
    {
        _connectionString = ConfigHelper.Instance.GetConfig().APIConfigs.AZURE_STORAGE_CONNECTION_STRING
                            ?? throw new InvalidOperationException("A variável de ambiente AZURE_STORAGE_CONNECTION_STRING não está definida.");

        _blobServiceClient = new BlobServiceClient(_connectionString);
    }

    public BlobServiceClient GetBlobServiceClient()
    {
        return _blobServiceClient;
    }

    public BlobContainerClient GetContainerClient(string containerName)
    {
        return _blobServiceClient.GetBlobContainerClient(containerName);
    }
}
}
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WindowsApp.Services.API;

using System.Text.Json;

namespace WindowsApp.Managers.Uploaders.Files
{
    public class AzureServices
    {
        /// <summary>
        /// Do upload readme file to azure clound blob storage
        /// </summary>
        /// <param name="NameFile">
        /// Name of file
        /// </param>
        /// <param name="Path">
        /// abolute file path 
        /// </param>
        public static async Task<bool> Uploader(string NameFile, string Path)
        {
            string containerName = "mdfilesproject";

            try
            {
                AzureAuthenticator authenticator = new AzureAuthenticator();
                BlobUploader uploader = new BlobUploader(authenticator, containerName);

                var response = await uploader.UploadFileAsync(Path, NameFile);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Azure Uploader: {ex}");
                return false;
            }

        }
    }

    public class BlobUploader
    {
        private readonly BlobContainerClient _containerClient;

        public BlobUploader(AzureAuthenticator authenticator, string containerName)
        {
            _containerClient = authenticator.GetContainerClient(containerName);
        }

        public async Task ListBlobsAsync()
        {
            Console.WriteLine("📂 Arquivos no container:");
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                Console.WriteLine($"📄 {blobItem.Name}");
            }
        }

        public async Task<bool> UploadFileAsync(string filePath, string blobName)
        {
            if (filePath == null || blobName == null)
            {
                return false;
            }

            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient(blobName);
                using (var newFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)){
                    var response = await blobClient.UploadAsync(newFileStream, overwrite: true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"BlobUploader : UploadFileAsync(), Error: {ex}");
            }
        }

        public async Task DownloadFileAsync(string blobName, string downloadPath)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            using (FileStream fileStream = File.OpenWrite(downloadPath))
            {
                await download.Content.CopyToAsync(fileStream);
            }
            Console.WriteLine($"📥 Download concluído: {downloadPath}");
        }
    }
}
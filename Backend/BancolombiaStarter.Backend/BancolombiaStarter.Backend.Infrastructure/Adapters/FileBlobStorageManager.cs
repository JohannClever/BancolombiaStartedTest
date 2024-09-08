using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BancolombiaStarter.Backend.Domain.Ports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace BancolombiaStarter.Backend.Infrastructure.Adapters
{
    public class FileBlobStorageManager: IFileBlobStorageManager
    {
        private readonly IConfiguration _configuration;

        public FileBlobStorageManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<BlobContainerClient> PrepareContainerAccessAsync(string blobContainer, bool isPrivate = false)
        {
            if (string.IsNullOrEmpty(blobContainer))
            {
                throw new Exception("The Container Name attribute is empty");
            }
            var conectionString = _configuration["Storage:ConectionString"];
            var container = new BlobContainerClient(conectionString, blobContainer.ToLowerInvariant());//el nombre del blobContainer no puede tener letras en mayuscula
            bool exists = await container.ExistsAsync();
            if (!exists)
                if (isPrivate)
                    await container.CreateAsync();
                else
                    await container.CreateAsync(PublicAccessType.BlobContainer);
            return container;
        }

        private byte[] GetFileBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task<string> SaveFileAsync(string contanerName, IFormFile formFile)
        {
            string resultUri = string.Empty;
            try
            {
                var fileContent = GetFileBytes(formFile);
                resultUri = await SaveFileAsync(contanerName, formFile.FileName, fileContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultUri;
        }

        public async Task<string> SaveFileAsync(string contanerName, string fileName, byte[] content)
        {
            var container = await PrepareContainerAccessAsync(contanerName);
            if (string.IsNullOrEmpty(fileName))
                return null;
            BlobClient blob = null;
            try
            {
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
                blob = container.GetBlobClient(fileName);
                using (var stream = new MemoryStream(content, writable: false))
                {
                    await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return blob.Uri.ToString();
        }

        public async Task<string> GetDataFromFileAsync(string blobContainer, string blobName)
        {
            var container = await PrepareContainerAccessAsync(blobContainer);
            if (string.IsNullOrEmpty(blobName))
                return null;
            BlobClient blob = null;
            try
            {
                blob = container.GetBlobClient(blobName);
                var response = await blob.DownloadContentAsync();
                return response.Value.Content.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> DeleteFileAsync(BlobClient blob)
        {
            bool result;
            try
            {
                var status = new int[] { 200, 202 };
                var response = await blob.DeleteAsync();
                result = status.Contains(response.Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public async Task<bool> DeleteFileAsync(string blobContainer, string blobName)
        {
            var container = await PrepareContainerAccessAsync(blobContainer);
            if (string.IsNullOrEmpty(blobName))
                return false;
            BlobClient blob = container.GetBlobClient(blobName);
            return await DeleteFileAsync(blob);
        }

        public async Task<bool> DeleteFileAsync(Uri blobUi)
        {
            return await DeleteFileAsync(blobUi);
        }
    }
}

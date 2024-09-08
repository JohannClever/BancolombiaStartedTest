using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BancolombiaStarter.Backend.Domain.Ports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace BancolombiaStarter.Backend.Domain.Ports
{
    public interface IFileBlobStorageManager
    {
        Task<string> SaveFileAsync(string contanerName, IFormFile formFile);

        Task<string> SaveFileAsync(string contanerName, string fileName, byte[] content);

        Task<string> GetDataFromFileAsync(string blobContainer, string blobName);

        Task<bool> DeleteFileAsync(string blobContainer, string blobName);

        Task<bool> DeleteFileAsync(Uri blobUi);
    }
}

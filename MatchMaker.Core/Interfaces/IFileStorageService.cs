using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Interfaces
{
    public interface IFileStorageService
    {
        bool CreateDirectory(string relativePath);
        Task<string> StoreFileAsync(IFormFile file, string subdirectory);
    }
}

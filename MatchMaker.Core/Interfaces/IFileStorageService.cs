using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> StoreFileAsync(IFormFile file, string subdirectory);
        void CreateDirectory(string relativePath);
        string GetFullPath(string relativePath);
    }
}

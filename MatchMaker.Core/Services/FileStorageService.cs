using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class FileStorageService(ILogger<FileStorageService> logger, string storageRootPath, string baseUrl, string storagePath = "storage/uploads") : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _storageRootPath = storageRootPath ?? throw new ArgumentNullException(nameof(_storageRootPath));
    private readonly string _baseUrl = (baseUrl ?? throw new ArgumentNullException(nameof(baseUrl))).TrimEnd('/');
    private readonly string _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
    
    public string GetFullPath(string relativePath)
    {
        return Path.Combine(_storageRootPath, _storagePath, relativePath);
    }

    public bool CreateDirectory(string relativePath)
    {
        var fullPath = Path.GetFullPath(GetFullPath(relativePath));
        var intendedRoot = Path.GetFullPath(Path.Combine(_storageRootPath, _storagePath));

        try
        {
            if (!fullPath.StartsWith(intendedRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Attempted path-traversal outside of the storage root");
            }
            return Directory.CreateDirectory(fullPath) != null;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "The caller doesn't have the authorization to create the path {Path}.", fullPath);
            throw;
        }
    }
    public Task<string> StoreFileAsync(IFormFile file, string subdirectory)
    {
        throw new NotImplementedException();
    }
}

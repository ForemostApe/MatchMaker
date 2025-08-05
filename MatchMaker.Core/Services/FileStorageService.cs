using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class FileStorageService(ILogger<FileStorageService> logger, string storageRootPath, string baseUrl, string storagePath = "storage/uploads") : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _storageRootPath = storageRootPath ?? throw new ArgumentNullException(nameof(storageRootPath));
    private readonly string _baseUrl = (baseUrl ?? throw new ArgumentNullException(nameof(baseUrl))).TrimEnd('/');
    private readonly string _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
    
    public async Task<string> StoreFileAsync(IFormFile file, string subdirectory)
    {
        if (file == null || file.Length == 0) 
            throw new ArgumentException("File cannot be null or empty", nameof(file));

        if (string.IsNullOrWhiteSpace(subdirectory)) 
            throw new ArgumentNullException(nameof(subdirectory), "Subdirectory cannot be null or empty");

        subdirectory = SanitizeSubdirectory(subdirectory);

        string safeFileName = SanitizeFileName(subdirectory, file.FileName);
        string relativePath = Path.Combine(_storagePath, subdirectory, safeFileName).Replace('\\', '/');
        string fullPath = EnsureSafeFullPath(relativePath);

        CreateDirectory(fullPath);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Path.Combine(_storagePath, relativePath).Replace('\\', '/');
    }

    private static string SanitizeSubdirectory(string subdirectory)
    {
        return subdirectory.Replace("..", string.Empty).Trim('/').Replace('\\', '/');
    }
    private static string SanitizeFileName(string subdirectory, string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(fileExtension))
            throw new ArgumentException("File must have a file-extension.", nameof(fileName));

        return $"{subdirectory}-{Guid.NewGuid()}.{fileExtension}";
    }

    private string EnsureSafeFullPath(string relativePath)
    {
        string fullPath = Path.GetFullPath(Path.Combine(_storageRootPath, _storagePath, relativePath));
        string rootPath = Path.GetFullPath(Path.Combine(_storageRootPath, _storagePath));
        if (!fullPath.StartsWith(Path.GetFullPath(rootPath), StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Attempted path-traversal outside of the storage root");

        return fullPath;
    }

    public void CreateDirectory(string fullPath)
    {
        try
        { 
            var existingDirectory = Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException("Could not determine directory from path");
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "The caller doesn't have the authorization to create the path {Path}.", fullPath);
            throw;
        }
    }

    public string GetFullPath(string relativePath)
    {
        return Path.Combine(_storageRootPath, _storagePath, relativePath);
    }
}

using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchMaker.Core.Services;

public class FileValidationService(ILogger<FileValidationService> logger, IOptions<FileValidationOptions> fileValidationOptions) : IFileValidationService
{
    private readonly ILogger<FileValidationService> _logger = logger;
    private readonly FileValidationOptions _fileValidationOptions = fileValidationOptions.Value;
    
    //TODO
    //Maybe assign header-values to appsettings as well and inject through IOptions.
    //Implement SixLabors.ImageSharp-library for more thorough check?
    
    private static readonly byte[] JpegHeader = [0xFF, 0xD8, 0xFF, 0xE0];
    private static readonly byte[] PngHeader = [0x89, 0x50, 0x4E, 0x47];
    
    public bool ValidateTeamLogoFile(IFormFile teamLogo)
    {
        if (!ValidateFileSize(teamLogo.Length)) 
            return false;
        
        if (!ValidateFileFormat(Path.GetExtension(teamLogo.FileName))) 
            return false;
        
        if (!ValidateFileSignature(teamLogo)) 
            return false;

        return true;
    }

    private bool ValidateFileFormat(string fileExtension)
        => _fileValidationOptions.ValidFileExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);

    private bool ValidateFileSignature(IFormFile file)
    {
        try
        {
            using var stream = file.OpenReadStream();
            Span<byte> header = stackalloc byte[8];
            int bytesRead = stream.Read(header);
            
            if (bytesRead < 3) 
                return false;
            
            if (header.StartsWith(JpegHeader)) 
                return true;
            
            if (bytesRead >= 4 && header.StartsWith(PngHeader)) 
                return true;

            return false;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate file signature.");
            throw;
        }
    }
    
    private bool ValidateFileSize(long fileLength)
        => fileLength <= _fileValidationOptions.MaxFileSize;
}
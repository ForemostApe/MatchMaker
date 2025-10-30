using MatchMaker.Core.Services;
using MatchMaker.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MatchMaker.Core.Test;

public class FileValidationUnitTests
{
    private readonly Mock<ILogger<FileValidationService>> _loggerMock = new();

    private static FileValidationOptions CreateFileValidationOptions() => new()
    {
        MaxFileSize = 2 * 1024 * 1024,
        ValidFileExtensions = [".jpg", ".jpeg", ".png"]
    };

    private FileValidationService CreateFileValidationService ()
    {
        var options = Options.Create(CreateFileValidationOptions());
        return new FileValidationService(_loggerMock.Object, options);
    }

    private static IFormFile CreateMockTeamLogoImageFile(int fileSizeInBytes, string fileName, byte[]? header =  null)
    {
        var content = new byte[fileSizeInBytes];

        if (header != null && header.Length <= content.Length)
        {
            Array.Copy(header, content, header.Length);
        }
        
        var stream = new MemoryStream(content);
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(x => x.OpenReadStream()).Returns(stream);
        mockFile.Setup(x => x.FileName).Returns(fileName);
        mockFile.Setup(x => x.Length).Returns(stream.Length);
        return mockFile.Object;
    }
    
    [Fact]
    public void ValidatePNGImageFile_ShouldReturnTrue_ForValidPNG()
    {
        //Arrange
        byte[] pngHeader = [0x89, 0x50, 0x4E, 0x47];
        var file = CreateMockTeamLogoImageFile(1000,"test.png", pngHeader);
        var service = CreateFileValidationService();

        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.True(testResult);
    }

    [Fact]
    public void ValidateJPGImageFile_ShouldReturnTrue_ForValidJPG()
    {
        //Arrange
        byte[] jpgHeader = [0xFF, 0xD8, 0xFF, 0xE0];
        var file = CreateMockTeamLogoImageFile(1000, "test.jpg", jpgHeader);
        var service = CreateFileValidationService();
        
        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.True(testResult);
    }

    [Fact]
    public void InvalidateFileSignature_ShouldReturnFalse_ForInvalidFileSignature()
    {
        //Arrange
        byte[] header = [0xFF, 0xFF, 0xFF, 0xFF];
        var file = CreateMockTeamLogoImageFile(1 * 1024 * 1024, "test.jpg", header);
        var service = CreateFileValidationService();
        
        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.False(testResult);
    }
    
    [Fact]
    public void ValidateFileExtensions_ShouldReturnTrue_ForValidFileExtensions()
    {
        //Arrange
        byte[] jpgHeader = [0xFF, 0xD8, 0xFF, 0xE0];
        var file = CreateMockTeamLogoImageFile(1 * 1024 * 1024, "test.jpeg", jpgHeader);
        var service = CreateFileValidationService();
        
        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.True(testResult);
    }

    [Fact]
    public void InvalidateFileExtensions_ShouldReturnFalse_ForInvalidFileExtensions()
    {
        //Arrange
        byte[] jpgHeader = [0xFF, 0xD8, 0xFF, 0xE0];
        var file = CreateMockTeamLogoImageFile(1 * 1024 * 1024, "test.apa", jpgHeader);
        var service = CreateFileValidationService();
        
        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.False(testResult);
    }

    [Fact]
    public void InvalidateFileSize_ShouldReturnFalse_ForInvalidFileSize()
    {
        //Arrange
        byte[] jpgHeader = [0xFF, 0xD8, 0xFF, 0xE0];
        var file = CreateMockTeamLogoImageFile(5 * 1024 * 1024, "test.jpg", jpgHeader);
        var service = CreateFileValidationService();
        
        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.False(testResult);
    }
}
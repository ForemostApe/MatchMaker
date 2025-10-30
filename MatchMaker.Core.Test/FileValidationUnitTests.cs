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

    private static IFormFile CreateMockTeamLogoImageFile(byte[] content, string fileName)
    {
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
        var file =  CreateMockTeamLogoImageFile(pngHeader, "test.png");
        var service = CreateFileValidationService();

        //Act
        var testResult = service.ValidateTeamLogoFile(file);
        
        //Assert
        Assert.True(testResult);
    }
}
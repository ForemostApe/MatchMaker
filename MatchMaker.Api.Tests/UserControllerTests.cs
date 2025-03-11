using Microsoft.AspNetCore.Mvc;
using Moq;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Api.Controllers;
using Microsoft.Extensions.Logging;

public class UserControllerTests
{
    private readonly Mock<ILogger<UserController>> _loggerMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _loggerMock = new Mock<ILogger<UserController>>();
        _userServiceMock = new Mock<IUserService>();
        _userController = new UserController(_loggerMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_ReturnsCreatedResult()
    {
        // Arrange
        var newUser = new CreateUserDTO { Email = "test@example.com", Password = "password" };
        var userResult = new UserResultDTO
        {
            IsSuccess = true,
            userDTO = new UserDTO { UserId = "123", Email = "test@example.com" }
        };

        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserDTO>()))
                         .ReturnsAsync(userResult);

        // Act
        var result = await _userController.CreateUserAsync(newUser);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal("123", createdResult.Value);
        Assert.NotNull(createdResult.Location);
    }

    [Fact]
    public async Task CreateUserAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var newUser = new CreateUserDTO { Email = "", Password = "" };
        _userController.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _userController.CreateUserAsync(newUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateUserAsync_ServiceFails_ReturnsConflict()
    {
        // Arrange
        var newUser = new CreateUserDTO { Email = "test@example.com", Password = "password" };
        var userResult = new UserResultDTO
        {
            IsSuccess = false,
            Message = "User already exists"
        };

        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserDTO>()))
                         .ReturnsAsync(userResult);

        // Act
        var result = await _userController.CreateUserAsync(newUser);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("User already exists", conflictResult.Value);
    }
}
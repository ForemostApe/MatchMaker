using Moq;
using MatchMaker.Core.Services;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MapsterMapper;

public class UserServiceTests
{
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserRepo> _userRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _loggerMock = new Mock<ILogger<UserService>>();
        _mapperMock = new Mock<IMapper>();
        _userRepoMock = new Mock<IUserRepo>();
        _userService = new UserService(_loggerMock.Object, _mapperMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var newUser = new UserDTO { Email = "test@example.com", Password = "password" };
        var userEntity = new User { Email = "test@example.com", Password = "password" };
        var createdUser = new User { ID = "123", Email = "test@example.com" };

        _mapperMock.Setup(x => x.Map<User>(newUser)).Returns(userEntity);
        _userRepoMock.Setup(x => x.CreateUserAsync(userEntity)).ReturnsAsync(true);
        _userRepoMock.Setup(x => x.GetUserByEmailAsync(userEntity.Email)).ReturnsAsync(createdUser);
        _mapperMock.Setup(x => x.Map<UserDTO>(createdUser)).Returns(new UserDTO { UserId = "123", Email = "test@example.com" });

        // Act
        var result = await _userService.CreateUserAsync(newUser);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("123", result.UserDTO.UserId);
    }

    [Fact]
    public async Task CreateUserAsync_RepoFails_ReturnsFailure()
    {
        // Arrange
        var newUser = new UserDTO { Email = "test@example.com", Password = "password" };
        var userEntity = new User { Email = "test@example.com", Password = "password" };

        _mapperMock.Setup(x => x.Map<User>(newUser)).Returns(userEntity);
        _userRepoMock.Setup(x => x.CreateUserAsync(userEntity)).ReturnsAsync(false);

        // Act
        var result = await _userService.CreateUserAsync(newUser);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Couldn't create user.", result.Message);
    }
}
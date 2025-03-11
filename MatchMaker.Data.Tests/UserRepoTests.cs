using Moq;
using MongoDB.Driver;
using MatchMaker.Data.Repositories;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

public class UserRepoTests
{
    private readonly Mock<ILogger<UserRepo>> _loggerMock;
    private readonly Mock<IMongoCollection<User>> _userCollectionMock;
    private readonly Mock<IMongoDatabase> _databaseMock;
    private readonly UserRepo _userRepo;

    public UserRepoTests()
    {
        _loggerMock = new Mock<ILogger<UserRepo>>();
        _userCollectionMock = new Mock<IMongoCollection<User>>();
        _databaseMock = new Mock<IMongoDatabase>();
        _databaseMock.Setup(x => x.GetCollection<User>("users", null)).Returns(_userCollectionMock.Object);
        _userRepo = new UserRepo(_loggerMock.Object, _databaseMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_ReturnsTrue()
    {
        // Arrange
        var newUser = new User { Email = "test@example.com", Password = "password" };

        _userCollectionMock.Setup(x => x.InsertOneAsync(newUser, null, default))
                           .Returns(Task.CompletedTask);

        // Act
        var result = await _userRepo.CreateUserAsync(newUser);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateUserAsync_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var newUser = new User { Email = "test@example.com", Password = "password" };

        _userCollectionMock.Setup(x => x.InsertOneAsync(newUser, null, default))
                           .ThrowsAsync(new MongoWriteException(new WriteError(11000, "Duplicate key", "Email")));

        // Act & Assert
        await Assert.ThrowsAsync<MongoWriteException>(() => _userRepo.CreateUserAsync(newUser));
    }
}
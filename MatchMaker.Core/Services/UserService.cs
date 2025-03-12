using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class UserService(ILogger<UserService> logger, IMapper mapper, IUserRepo userRepo) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<Result<UserDTO>> CreateUserAsync(CreateUserDTO newUser)
    {
        try
        {
            var user = _mapper.Map<User>(newUser);

            bool result = await _userRepo.CreateUserAsync(user);

            if (!result) return Result<UserDTO>.Failure("Couldn't create user");

            var createdUser = await _userRepo.GetUserByEmailAsync(user.Email);

            if (createdUser == null)
            {
                _logger.LogError("Couldn't fetch newly created user {email} from database after writing to database.", newUser.Email);
                throw new Exception("Unexpected error when trying to fetch newly created user after writing to database");
            }

            UserDTO userDTO = _mapper.Map<UserDTO>(createdUser);

            return Result<UserDTO>.Success(userDTO, "User successfully created.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to apply logic when creating account for {email}", newUser.Email);
            throw;
        }
    }

    public async Task<Result<UserDTO>> GetUserByIdAsync(string userId)
    {
        try
        {
            var existingUser = await _userRepo.GetUserByIdAsync(userId);

            if (existingUser == null) return Result<UserDTO>.Failure("Couldn't find user");

            var fetchedUser = _mapper.Map<UserDTO>(existingUser);

            return Result<UserDTO>.Success(fetchedUser, "User successfully found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to get user {userId}", userId);
            throw;
        }
    }
}
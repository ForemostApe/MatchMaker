using MapsterMapper;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class UserService(ILogger<UserService> logger, IMapper mapper, IUserRepo userRepo)
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<UserResultDTO> CreateUserAsync(UserDTO newUser)
    {
        try
        {
            var user = _mapper.Map<UserEntity>(newUser);

            bool result = await _userRepo.CreateUserAsync(user);
            if (!result)
            {
                return new UserResultDTO()
                {
                    IsSuccess = false,
                    Message = "Couldn't create user."
                };
            }

            var createdUser = await _userRepo.GetUserByEmailAsync(user.Email);

            if (createdUser == null)
            {
                _logger.LogError("Couldn't fetch newly created user {email} from database after writing to database.", newUser.Email);
                throw new Exception("Unexpected error when trying to fetch newly created user after writing to database");
            }
            return new UserResultDTO()
            {
                IsSuccess = true,
                Message = "User successfully created.",
                UserDTO = _mapper.Map<UserDTO>(createdUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to apply logic when creating account for {email}", newUser.Email);
            throw;
        }
    }

    public async Task<UserResultDTO> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new UserResultDTO()
                {
                    IsSuccess = false,
                    Message = "User couldn't be found."
                };
            }

            return new UserResultDTO()
            {
                IsSuccess = true,
                Message = "User found.",
                UserDTO = _mapper.Map<UserDTO>(user)
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to get user {userId}", userId);
            throw;
        }
    }
}
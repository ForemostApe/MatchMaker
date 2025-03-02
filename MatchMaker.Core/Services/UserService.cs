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

            return new UserResultDTO()
            {
                IsSuccess = true,
                Message = "User successfully created.",
                userDTO = _mapper.Map<UserDTO>(createdUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to apply logic when creating account for {email}", newUser.Email);
            throw;
        }
    }
}

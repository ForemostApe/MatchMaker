using DnsClient.Internal;
using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Teams;
using MatchMaker.Domain.DTOs.Users;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Facades;

public class UserServiceFacade(ILogger<UserServiceFacade> logger, IMapper mapper, IUserService userService, IEmailService emailService, ITokenService tokenService) : IUserServiceFacade
{
    private readonly ILogger<UserServiceFacade> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<UserDTO>> CreateUserAsync(CreateUserDTO newUser)
    {
        var user = _mapper.Map<User>(newUser);
        var result = await _userService.CreateUserAsync(user);

        if (result == null) return Result<UserDTO>.Failure("User already exists.");

        string verificationToken = await _tokenService.GenerateVerificationToken(result);
        await _emailService.CreateEmailAsync(result.Email, EmailService.EmailType.UserCreated, verificationToken);

        UserDTO createdUser = _mapper.Map<UserDTO>(user);

        return Result<UserDTO>.Success(createdUser, "User successfully created.");
    }

    public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return Result<UserDTO>.Failure("Couldn't find user");

            var existingUser = _mapper.Map<UserDTO>(user);

            return Result<UserDTO>.Success(existingUser, "User successfully found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to get user with email-address {Email}", email);
            throw new ApplicationException($"An unexpected error occurred while trying to get user with email-address {email}", ex);
        }
    }

    public async Task<Result<UserDTO>> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return Result<UserDTO>.Failure("User couldn't be found.");

            var existingUser = _mapper.Map<UserDTO>(user);

            return Result<UserDTO>.Success(existingUser, "User successfully found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to get user {UserId}", userId);
            throw new ApplicationException($"An unexpected error occurred while trying to get user with user-id {userId}", ex);
        }
    }
    public async Task<Result<UserDTO>> UpdateUserAsync(UpdateUserDTO userUpdate)
    {
        ArgumentNullException.ThrowIfNull(userUpdate);      

        try
        {
            var existingUser = await _userService.GetUserByEmailAsync(userUpdate.Email);

            if (existingUser == null) return Result<UserDTO>.Failure("User couldn't be found.");

            if (existingUser.Data != null && !existingUser.Data!.Id.Equals(userUpdate.Id))
            {
                return Result<UserDTO>.Failure("Email already exists.");
            }            

            userUpdate.Adapt(existingUser.Data);
            
            var result = await _userService.UpdateUserAsync(existingUser.Data!);

            if (!result.IsSuccess) return Result<UserDTO>.Failure(result.Message);

            var updatedUser = result.Data!.Adapt<UserDTO>();

            return Result<UserDTO>.Success(updatedUser, "User successfully updated.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(string userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Result<bool>.Success(true, "user successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to delete user with id {userId}", userId);
            throw new ApplicationException($"An unexpected error occurred while trying to delete user with id {userId}", ex);
        }
    }
}

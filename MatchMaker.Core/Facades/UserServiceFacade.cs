using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Users;
using MatchMaker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Facades;

public class UserServiceFacade(ILogger<UserServiceFacade> logger, IMapper mapper, IUserService userService, IEmailService emailService, ITokenService tokenService, IAuthService authService) : IUserServiceFacade
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserServiceFacade> _logger = logger;
    private readonly IEmailService _emailService = emailService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAuthService _authService = authService;

    public async Task<Result<UserDTO>> CreateUserAsync(CreateUserDTO newUser)
    {
        ArgumentNullException.ThrowIfNull(newUser);

        try
        {
            var existingUser = await _userService.GetUserByEmailAsync(newUser.Email);
            if (existingUser.IsSuccess)
            {
                return Result<UserDTO>.Failure(
                    "User already exists.",
                    existingUser.Message,
                    StatusCodes.Status409Conflict
                );
            }

            var user = _mapper.Map<User>(newUser);
            user.PasswordHash = _authService.HashPassword(newUser.Password);

            var result = await _userService.CreateUserAsync(user);

            if (!result.IsSuccess)
            {
                return Result<UserDTO>.Failure(
                    "User-creation failed.",
                    result.Message,
                    StatusCodes.Status500InternalServerError
                );
            }

            string verificationToken = await _tokenService.GenerateVerificationToken(result.Data!);
            await _emailService.CreateEmailAsync(result.Data!.Email, EmailService.EmailType.UserCreated, verificationToken);

            var createdUser = _mapper.Map<UserDTO>(result.Data);

            return Result<UserDTO>.Success(createdUser, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to create user.");
            throw;
        }
    }

    public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (!user.IsSuccess) return Result<UserDTO>.Failure(user.Message);

            var existingUser = _mapper.Map<UserDTO>(user.Data!);

            return Result<UserDTO>.Success(existingUser, user.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to get user by email-address.");
            throw;
        }
    }

    public async Task<Result<UserDTO>> GetUserByIdAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (!user.IsSuccess) return Result<UserDTO>.Failure(user.Message);

            var existingUser = _mapper.Map<UserDTO>(user.Data!);

            return Result<UserDTO>.Success(existingUser, user.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to get user by UserId.");
            throw;
        }
    }

    public async Task<Result<List<UserDTO>>> GetUsersByRole(string userRole, string? teamId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(userRole);

        try
        {
            if (!Enum.TryParse<UserRole>(userRole, true, out var parsedRole)) throw new ArgumentException($"Invalid user role: {userRole}");

            var result = await _userService.GetUsersByRoleAsync(parsedRole, teamId);
            if (!result.IsSuccess) return Result<List<UserDTO>>.Failure(result.Message);

            var users = _mapper.Map<List<UserDTO>>(result.Data!);

            return Result<List<UserDTO>>.Success(users, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to get user by UserRole.");
            throw;
        }
    }

    public async Task<Result<UserDTO>> UpdateUserAsync(UpdateUserDTO userUpdate)
    {
        ArgumentNullException.ThrowIfNull(userUpdate);

        try
        {
            var existingUser = await _userService.GetUserByIdAsync(userUpdate.Id);

            if (!existingUser.IsSuccess)
            {
                return Result<UserDTO>.Failure(
                    "User doesn't exist",
                    existingUser.Message,
                    StatusCodes.Status404NotFound
                    );
            }

            if (userUpdate.Email != null)
            {
                var existingEmailAddress = await _userService.GetUserByEmailAsync(userUpdate.Email);
                if (existingEmailAddress.IsSuccess && existingEmailAddress.Data!.Id != userUpdate.Id)
                {
                    return Result<UserDTO>.Failure(
                        "Email already in use",
                        "Another user is already registered with this email address.",
                        StatusCodes.Status409Conflict
                    );
                }
            }

            userUpdate.Adapt(existingUser.Data);

            var result = await _userService.UpdateUserAsync(existingUser.Data!);

            return result.IsSuccess
                ? Result<UserDTO>.Success(result.Data!.Adapt<UserDTO>(), result.Message)
                : Result<UserDTO>.Failure(
                    "Update failed.",
                    result.Message,
                    StatusCodes.Status500InternalServerError
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to update user.");
            throw;
        }
    }

    public async Task<Result<UserDTO>> DeleteUserAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        try
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result.IsSuccess
                ? Result<UserDTO>.Success(_mapper.Map<UserDTO>(result.Data!), result.Message)
                : Result<UserDTO>.Failure(
                    "Deletion failed.",
                    result.Message,
                    StatusCodes.Status500InternalServerError
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the userservice-facade while trying to delete user.");
            throw;
        }
    }
}

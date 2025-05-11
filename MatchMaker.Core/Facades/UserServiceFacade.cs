using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Domain.DTOs;
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
        ArgumentNullException.ThrowIfNull(newUser);

        try
        {
            var user = _mapper.Map<User>(newUser);
            var result = await _userService.CreateUserAsync(user);

            if (!result.IsSuccess) return Result<UserDTO>.Failure(result.Message);

            string verificationToken = await _tokenService.GenerateVerificationToken(result.Data!);
            await _emailService.CreateEmailAsync(result.Data!.Email, EmailService.EmailType.UserCreated, verificationToken);

            var createdUser = _mapper.Map<UserDTO>(result.Data);

            return Result<UserDTO>.Success(createdUser, result.Message);
        }
        catch
        {
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
        catch
        {
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
        catch
        {
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
        catch
        {
            throw;
        }
    }

    public async Task<Result<UserDTO>> UpdateUserAsync(UpdateUserDTO userUpdate)
    {
        ArgumentNullException.ThrowIfNull(userUpdate);      

        try
        {
            var existingUser = await _userService.GetUserByIdAsync(userUpdate.Id);

            if (!existingUser.IsSuccess) return Result<UserDTO>.Failure(existingUser.Message);

            
            //Det här är fel! Ska kolla så att inte mailadressen redan finns och inget annat!!!
            //if (existingUser.Data != null && !existingUser.Data!.Email.Equals(userUpdate.Email))
            //{
            //    return Result<UserDTO>.Failure("Email already exists.");
            //}            

            userUpdate.Adapt(existingUser.Data);
            
            var result = await _userService.UpdateUserAsync(existingUser.Data!);

            if (!result.IsSuccess) return Result<UserDTO>.Failure(result.Message);

            var updatedUser = result.Data!.Adapt<UserDTO>();

            return Result<UserDTO>.Success(updatedUser, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<UserDTO>> DeleteUserAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        try
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (!result.IsSuccess) return Result<UserDTO>.Failure(result.Message);

            var userDTO = _mapper.Map<UserDTO>(result.Data!);

            return Result<UserDTO>.Success(userDTO, result.Message);
        }
        catch
        {
            throw;
        }
    }
}

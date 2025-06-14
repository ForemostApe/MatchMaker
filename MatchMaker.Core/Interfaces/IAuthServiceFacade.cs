﻿using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Authentication;

namespace MatchMaker.Core.Interfaces;
public interface IAuthServiceFacade
{
    Task<Result<bool>> VerifyEmailAsync(string token);
    Task<Result<AuthenticationDTO>> LoginAsync(LoginDTO loginDTO);
    Result<string> Logout();
    Task<Result<AuthenticationDTO>> GenerateRefreshTokenAsync(string refreshToken);
}

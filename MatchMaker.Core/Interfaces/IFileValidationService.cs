using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Interfaces;

public interface IFileValidationService
{
    bool ValidateTeamLogoFile(IFormFile teamLogo);
}
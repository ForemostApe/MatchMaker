namespace MatchMaker.Core.Interfaces;
public interface IAuthService
{
    string HashPassword(string password);
}
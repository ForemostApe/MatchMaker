namespace MatchMaker.Core.Interfaces;

public interface ILinkFactory
{
    string CreateVerificationLink(string userId);

    string CreateResetPasswordLink(string email);
}

namespace MatchMaker.Core.Interfaces;

public interface IVerificationLinkFactory
{
    string CreateVerificationLink(string userId);
}

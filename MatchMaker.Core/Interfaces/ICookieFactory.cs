namespace MatchMaker.Core.Interfaces;

public interface ICookieFactory
{
    void CreateHttpOnlyCookie(string tokenName, string token);
    void DeleteCookie(string tokenName);
}

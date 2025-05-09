namespace MatchMaker.Core.Interfaces;

public interface ICookieFactory
{
    void CreateHttpOnlyCookie(string tokenName, string token);
    void ExpireCookie(string tokenName);
}

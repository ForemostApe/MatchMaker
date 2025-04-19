namespace MatchMaker.Core.Interfaces;

public interface IEmailTemplateEngine
{
    public string RenderTemplate<T>(string templateName, T model);
}

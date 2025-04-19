using MatchMaker.Core.Interfaces;
using Scriban;
using System.Reflection;

namespace MatchMaker.Core.Services;

public class EmailTemplateEngine : IEmailTemplateEngine
{
    private readonly Assembly _assembly;
    private readonly string _rootNamespace;

    public EmailTemplateEngine()
    {
        _assembly = Assembly.GetExecutingAssembly();
        _rootNamespace = _assembly.GetName().Name!;

        LogEmbeddedResources();
    }

    public string RenderTemplate<T>(string templateName, T model)
    {
        var resourcePath = $"{_rootNamespace}.EmailTemplates.{templateName}.sbhtml";

        using var stream = _assembly.GetManifestResourceStream(resourcePath) ?? throw new FileNotFoundException($"Embedded template '{resourcePath}' not found. " + $"Available resources: {string.Join(", ", _assembly.GetManifestResourceNames())}");

        using var reader = new StreamReader(stream);
        string templateText = reader.ReadToEnd();

        if (string.IsNullOrWhiteSpace(templateText))
            throw new InvalidDataException($"Template '{resourcePath}' is empty");

        var template = Template.Parse(templateText);
        if (template == null) throw new InvalidOperationException($"Failed to parse template '{resourcePath}'");

        return template.Render(model, member => member.Name);
    }

    private void LogEmbeddedResources()
    {
#if DEBUG
        Console.WriteLine("Available embedded resources:");
        foreach (var name in _assembly.GetManifestResourceNames())
        {
            Console.WriteLine($"- {name}");
        }
#endif
    }
}
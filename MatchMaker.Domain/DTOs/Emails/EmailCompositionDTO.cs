namespace MatchMaker.Domain.DTOs.Emails
{
    public class EmailCompositionDTO(string templateName, string subject, object templateModel)
    {
        public string TemplateName { get; init; } = templateName;
        public string Subject { get; init; } = subject;
        public object TemplateModel { get; init; } = templateModel;
    }
}

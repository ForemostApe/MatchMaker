using static MatchMaker.Core.Services.EmailService;

namespace MatchMaker.Core.Interfaces
{
    public interface IEmailComposer
    {
        public (string TemplateName, string Subject, object Model) Compose(EmailType mailType, string email, string? token);
    }
}

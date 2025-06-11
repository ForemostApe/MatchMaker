using MatchMaker.Domain.DTOs.Emails;
using MatchMaker.Domain.Enums;

namespace MatchMaker.Core.Interfaces
{
    public interface IEmailComposer
    {
        EmailCompositionDTO Compose(EmailType mailType, string email, string? token);
    }
}

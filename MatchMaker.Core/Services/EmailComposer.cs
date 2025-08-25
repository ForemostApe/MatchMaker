using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using MatchMaker.Domain.DTOs.Emails;
using MatchMaker.Domain.Enums;

namespace MatchMaker.Core.Services
{
    public class EmailComposer(ILinkFactory linkFactory, ClientSettings clientSettings) : IEmailComposer
    {
        private readonly ILinkFactory _linkFactory = linkFactory;
        private readonly ClientSettings _clientSettings = clientSettings;

        public EmailCompositionDTO Compose(EmailType mailType, string email, string? token)
        {

            //if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Token is null when trying to create link.");

            return mailType switch
            {
                EmailType.UserCreated => new EmailCompositionDTO (
                    "UserCreatedTemplate",
                    "Ditt MatchMaker-konto har skapats.",
                    new
                    {
                        verification_link = _linkFactory.CreateVerificationLink(token)
                    }
                ),

                EmailType.PasswordReset => new EmailCompositionDTO (
                    "PasswordResetTemplate",
                    "Begäran att återställa MatchMaker-lösenord.",
                    new
                    {
                        resetPassword_link = _linkFactory.CreateResetPasswordLink(email!)
                    }
                ),

                EmailType.GameNotification => new EmailCompositionDTO (
                    "GameNotificationTemplate",
                    "En planerad match inväntar bedömning.",
                    new
                    {
                        login_link = _clientSettings.BaseURL
                    }
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(mailType))
            };
        }
    }
}

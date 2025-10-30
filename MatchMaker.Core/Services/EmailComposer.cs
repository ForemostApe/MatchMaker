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

        public EmailCompositionDto Compose(EmailType mailType, string email, string? token)
        {

            //if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Token is null when trying to create link.");

            return mailType switch
            {
                EmailType.UserCreated => new EmailCompositionDto (
                    "UserCreatedTemplate",
                    "Ditt MatchMaker-konto har skapats.",
                    new
                    {
                        verification_link = _linkFactory.CreateVerificationLink(token)
                    }
                ),

                EmailType.PasswordReset => new EmailCompositionDto (
                    "PasswordResetTemplate",
                    "Begäran att återställa MatchMaker-lösenord.",
                    new
                    {
                        resetPassword_link = _linkFactory.CreateResetPasswordLink(email!)
                    }
                ),

                EmailType.GameNotification => new EmailCompositionDto (
                    "GameNotificationTemplate",
                    "En planerad match inväntar bedömning.",
                    new
                    {
                        login_link = _clientSettings.BaseUri
                    }
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(mailType))
            };
        }
    }
}

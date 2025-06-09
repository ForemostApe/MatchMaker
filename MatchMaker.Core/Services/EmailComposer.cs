using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using static MatchMaker.Core.Services.EmailService;

namespace MatchMaker.Core.Services
{
    public class EmailComposer (ILinkFactory linkFactory, ClientSettings clientSettings) : IEmailComposer
    {
        private readonly ILinkFactory _linkFactory = linkFactory;

        public (string TemplateName, string Subject, object Model) Compose(EmailType mailType, string email, string? token)
        {
            return mailType switch
            {
                EmailType.UserCreated => (
                    "UserCreatedTemplate",
                    "Ditt MatchMaker-konto har skapats.",
                    new
                    {
                        verification_link = !string.IsNullOrEmpty(token)
                            ? _linkFactory.CreateVerificationLink(token)
                            : throw new ArgumentNullException("Token is null when trying to create verification-link.")
                    }
                ),

                EmailType.PasswordReset => (
                    "PasswordResetTemplate",
                    "Begäran att återställa MatchMaker-lösenord.",
                    new
                    {
                        resetPassword_link = !string.IsNullOrEmpty(email)
                        ? _linkFactory.CreateResetPasswordLink(email!)
                        : throw new ArgumentNullException("Email is null when trying create reset password-link.")
                    }
                ),

                EmailType.GameNotification => (
                    "GameNotificationTemplate",
                    "En planerad match inväntar bedömning.",
                    new
                    {
                        login_link = !string.IsNullOrEmpty(email)
                        ? clientSettings.BaseURL
                        : throw new ArgumentNullException("Email is null when trying create reset password-link.")
                    }
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(mailType))
            };
        }
    }
}

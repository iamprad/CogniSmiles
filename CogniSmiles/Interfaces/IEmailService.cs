using CogniSmiles.Services;

namespace CogniSmiles.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(EmailType emailType, Dictionary<string, object> config);
    }
}


using CogniSmiles.Services;

namespace CogniSmiles.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(EmailType emailType, string UserID, string toEmail, int? patientId = null);
    }
}


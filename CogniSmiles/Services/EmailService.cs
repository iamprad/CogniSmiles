using CogniSmiles.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.FileProviders;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using CogniSmiles.Data;
using CogniSmiles.Models;
using PayPal.Api;
using System.Numerics;

namespace CogniSmiles.Services
{
    public class EmailService: IEmailService
    {
        private readonly CogniSmilesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;
        public EmailService(CogniSmilesContext context, IWebHostEnvironment hostEnvironment, IConfiguration config)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _config = config;
        }
        public bool SendEmail(EmailType emailType, Dictionary<string, object> config) // string UserID, string toEmail, int? patientId = null,int? docId = null, string userName = null)
        {
            string fileName = string.Empty;
            string subject = string.Empty;
            string forgottenContent = string.Empty;
            var userId = config.GetValueOrDefault("UserID");
            switch (emailType)
            {
                case EmailType.Activation:
                    fileName = "ActivationEmail.html";
                    subject = "Activate Your CogniSmiles Account";
                    break;
                case EmailType.Notification:
                    fileName = "NotificationEmail.html";
                    subject = "Patient Status Change - CogniSmiles";
                    break;
                case EmailType.ForogttenUserName:
                    fileName = "ForgottenCredentials.html";
                    subject = "Your Registered Username with CogniSmiles";
                    forgottenContent = $"You Have requested to retrieve your username registered with Cognismiles Website. Your User Name is <b>{config.GetValueOrDefault("UserName")}</b>";
                    break;
                case EmailType.ForogttenPassword:
                    fileName = "ForgottenCredentials.html";
                    subject = "Your Reset Password Link - CogniSmiles";
                    forgottenContent = $"You Have requested to reset your password in Cognismiles Website. Your Unique link to reset your password is <a href=\"https://[DomainName]/Doctors/ResetCredentials?userId={userId}\"> Reset Password </a>.";
                    break;
                case EmailType.CourseRegistration:
                    fileName = "CourseRegistration.html";
                    subject = "New Course Regisration with Payment";
                    break;
                default:
                    break;
            }

            // configure email data
            var emailConfig = _config.GetSection("EmailConfiguration");

            var toEmail = config.GetValueOrDefault("ToEmail");

            var docId = config.GetValueOrDefault("DoctorID");

            if (string.IsNullOrEmpty(toEmail?.ToString()))
                toEmail = _context.Doctor.Where(d1 => d1.Id == Convert.ToInt32(docId)).Select(d => d.Email).FirstOrDefault();

            string configToEmail = emailConfig.GetValue<string>("ToEmail");
            if (!string.IsNullOrEmpty(configToEmail))
                toEmail = configToEmail;
            // get email template
            var fileProvider = new PhysicalFileProvider(_hostEnvironment.WebRootPath);
            var filePath = Path.Combine("Email Templates", fileName);
            var fileContents = fileProvider.GetFileInfo(filePath);

            if (!fileContents.Exists)
                return false;

            // replace in email contents

            var filestream = new FileStream(fileContents.PhysicalPath, FileMode.Open, FileAccess.Read);
            var emailContents = new StreamReader(filestream).ReadToEnd();

            if (userId != null)
                emailContents = emailContents.Replace("{{UserID}}", userId.ToString());

            var domainName = _config.GetValue<string>("DomainName");
            emailContents = emailContents.Replace("{{DomainName}}", domainName);
            var patientId = config.GetValueOrDefault("PatientID");

            if (patientId != null)
            {
                var patient = _context.Patient.Where(p => p.Id == Convert.ToInt32(patientId)).FirstOrDefault();
                if (patient != null)
                {
                    var doctorId = patient.DoctorId;
                    if (docId != null)
                        doctorId = (int)docId;
                    var doctor = _context.Doctor.Where(d => d.Id == doctorId).FirstOrDefault();

                    emailContents = emailContents.Replace("{{PatientCode}}", patient?.PatientCode);
                    emailContents = emailContents.Replace("{{PatientStatus}}", patient?.PatientStatus.ToString());
                    emailContents = emailContents.Replace("{{PracticeName}}", doctor?.PracticeName);
                    emailContents = emailContents.Replace("{{PatientID}}", patientId.ToString());
                }
            }

            var payStatus = config.GetValueOrDefault("PaymentStatus");
            if (payStatus != null)
            {
                subject += $" [{payStatus}]";
                if (payStatus.ToString() == "Success")
                {
                    emailContents = emailContents.Replace("{{Message1}}", " New Payment has been received for the Course ");
                    emailContents = emailContents.Replace("{{Message2}}", "Payee and Payment Details will be found in Stripe Dasbhoard");
                }
                else {
                    emailContents = emailContents.Replace("{{Message1}}", " Payment Attempt is failed for the Course ");
                    emailContents = emailContents.Replace("{{Message2}}", "Please verify Payee and Payment Details in Stripe Dasbhoard. Contact Payee if needed");
                }            
            }
            
            forgottenContent = forgottenContent.Replace("[DomainName]", domainName);
            
            emailContents = emailContents.Replace("{{EmailSubject}}", subject);
            emailContents = emailContents.Replace("{{EmailContent}}", forgottenContent);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailConfig.GetValue<string>("From")));
            email.To.Add(MailboxAddress.Parse(toEmail?.ToString()));
            string subjectPrefix = emailConfig.GetValue<string>("SubjectPrefix");
            if (!string.IsNullOrEmpty(subjectPrefix))
                subject = subjectPrefix + subject;
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailContents };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(emailConfig.GetValue<string>("SmtpServer"), emailConfig.GetValue<int>("Port"), SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(emailConfig.GetValue<string>("Username"), emailConfig.GetValue<string>("Password"));
            smtp.Send(email);
            smtp.Disconnect(true);
            return true;
        }
    }
    public enum EmailType
    {
        Activation,
        Notification,
        ForogttenUserName,
        ForogttenPassword,
        CourseRegistration
    }
}

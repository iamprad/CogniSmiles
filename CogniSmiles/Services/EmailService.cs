﻿using CogniSmiles.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.FileProviders;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using CogniSmiles.Data;
using CogniSmiles.Models;

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
        public bool SendEmail(EmailType emailType, string UserID, string toEmail, int? patientId = null)
        {
            string fileName = string.Empty;
            string subject = string.Empty;
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
                default:
                    break;
            }
            
            // configure email data
            var emailConfig = _config.GetSection("EmailConfiguration");
            string configToEmail = emailConfig.GetValue<string>("ToEmail");
            if (!string.IsNullOrEmpty(configToEmail))
                toEmail = configToEmail;
            // get email template
            var fileProvider = new PhysicalFileProvider(_hostEnvironment.WebRootPath);
            var filePath = Path.Combine("Email Templates",fileName);
            var fileContents = fileProvider.GetFileInfo(filePath);

            if (!fileContents.Exists)
                return false;
            
            // replace in email contents

            var filestream = new FileStream(fileContents.PhysicalPath, FileMode.Open, FileAccess.Read);
            var emailContents = new StreamReader(filestream).ReadToEnd();
            if(UserID != null)
                emailContents = emailContents.Replace("{{UserID}}", UserID);
            emailContents = emailContents.Replace("{{DomainName}}",_config.GetValue<string>("DomainName"));

            if(patientId != null)
            {
                var patient = _context.Patient.Where(p => p.Id == patientId).FirstOrDefault();
                if (patient != null)
                {
                    var doctor = _context.Doctor.Where(d => d.Id == patient.DoctorId).FirstOrDefault();

                    emailContents = emailContents.Replace("{{PatientCode}}", patient?.PatientCode);
                    emailContents = emailContents.Replace("{{PracticeName}}", doctor?.PracticeName);
                    emailContents = emailContents.Replace("{{PatientID}}", patientId.ToString());
                }
            }                 

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailConfig.GetValue<string>("From")));
            email.To.Add(MailboxAddress.Parse(toEmail));
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
        Notification
    }
}
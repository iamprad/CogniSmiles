using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;
using MailKit.Security;
using Microsoft.Extensions.FileProviders;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace CogniSmiles.Pages.Doctors
{
    public class ActivationModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;
        public ActivationModel(CogniSmilesContext context, IWebHostEnvironment hostEnvironment, IConfiguration config)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _config = config;
        }

        // [BindProperty]
        //public Doctor Doctor { get; set; } = default!;
        public string? DoctorEmail { get; set; }
        public Guid? UserId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Doctor == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            var login = await _context.Login.FirstOrDefaultAsync(m => m.DoctorId == id);
            if (login == null)
            {
                return NotFound();
            }
            DoctorEmail = doctor.Email;
            // send email to registered email address with activation instructions and activation link with userid

            // get email template
            var fileProvider = new PhysicalFileProvider(_hostEnvironment.WebRootPath);
            var filePath = Path.Combine("Email Templates", "ActivationEmail.html");
            var fileContents = fileProvider.GetFileInfo(filePath);

            if (!fileContents.Exists)
                return NotFound();

            // replace UserID in email content
            var filestream = new FileStream(fileContents.PhysicalPath, FileMode.Open, FileAccess.Read);
            var emailContents = new StreamReader(filestream).ReadToEnd();
            emailContents = emailContents.Replace("{{UserID}}", login.UserId.ToString());

            // configure email data

            var emailConfig = _config.GetSection("EmailConfiguration");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailConfig.GetValue<string>("From")));
            email.To.Add(MailboxAddress.Parse(DoctorEmail));
            email.Subject = "Activate Your CogniSmiles Account";
            email.Body = new TextPart(TextFormat.Html) { Text = emailContents };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(emailConfig.GetValue<string>("SmtpServer"), emailConfig.GetValue<int>("Port"), SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(emailConfig.GetValue<string>("Username"), emailConfig.GetValue<string>("Password"));
            smtp.Send(email);
            smtp.Disconnect(true);

            return Page();
        }
        public async Task<IActionResult> OnGetActivate(Guid userId)
        {

            var login = _context.Login.Where(l => l.UserId == userId).FirstOrDefault();
            if (login == null)
                return NotFound();
            
            login.IsActive = true;
            login.ActivatedOn = DateTime.Now;

            _context.Login.Update(login);
            await _context.SaveChangesAsync();
            UserId = login.UserId;
            DoctorEmail = login.UserName;

            return Page();
        }


    }
}

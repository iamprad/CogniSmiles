using CogniSmiles.Data;
using CogniSmiles.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace CogniSmiles.Pages.Dashboard
{
    public class UpdatePatientModel : AuthModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;
        private PatientStatus _patientStatus;
        public UpdatePatientModel(CogniSmilesContext context, IWebHostEnvironment hostEnvironment, IConfiguration config)
        {
            _context = context;           
            _hostEnvironment = hostEnvironment;
            _config = config;
        }
       
        [BindProperty]
        public Patient Patient { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!IsAuthenticated && (id == null || _context.Patient == null))
            {
                return NotFound();
            }

            var patient = await _context.Patient.FirstOrDefaultAsync(m => m.Id == id);
            
            if (patient == null)
            {
                return NotFound();
            }

            _patientStatus = patient.PatientStatus;
            Patient = patient;
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Patient).State = EntityState.Modified;

            try
            {
                if(Patient.PatientStatus != _patientStatus)
                {
                    // send email notification to aditya
                    // get email template
                    var fileProvider = new PhysicalFileProvider(_hostEnvironment.WebRootPath);
                    var filePath = Path.Combine("Email Templates", "NotificationEmail.html");
                    var fileContents = fileProvider.GetFileInfo(filePath);

                    if (!fileContents.Exists)
                        return NotFound();

                    // replace UserID in email content
                    var filestream = new FileStream(fileContents.PhysicalPath, FileMode.Open, FileAccess.Read);
                    var emailContents = new StreamReader(filestream).ReadToEnd();
                    emailContents = emailContents.Replace("{{PatientCode}}", Patient.PatientCode);
                    var doctor = _context.Doctor.Where(d => d.Id == Patient.DoctorId).FirstOrDefault();
                    if (doctor != null)
                    {
                        emailContents = emailContents.Replace("{{PracticeName}}", doctor.PracticeName);
                    }
                    emailContents = emailContents.Replace("{{PatientID}}", Patient.Id.ToString());
                    // configure email data

                    var emailConfig = _config.GetSection("EmailConfiguration");

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(emailConfig.GetValue<string>("From")));

                    var adminId = _context.Login.Where(l => l.AuthType == AuthType.Admin).Select(d => d.DoctorId).FirstOrDefault();
                    var docEmail = _context.Doctor.Where(d1 => d1.Id == adminId).Select(d => d.Email).FirstOrDefault();

                    if (IsAdmin && Patient.PatientStatus == (PatientStatus.AmendPlan | PatientStatus.ApprovePlan))
                    {
                        //Get Doctor Email
                        docEmail = _context.Doctor.Where(d1 => d1.Id == Patient.DoctorId).Select(d => d.Email).FirstOrDefault();                       
                    }
                    email.To.Add(MailboxAddress.Parse(docEmail));
                    //email.To.Add(MailboxAddress.Parse("pradeepvoorukonda@ymail.com"));

                    email.Subject = "[TEST Purpose Only] Patient Status Change - CogniSmiles";
                    email.Body = new TextPart(TextFormat.Html) { Text = emailContents };

                    // send email
                    using var smtp = new SmtpClient();
                    smtp.Connect(emailConfig.GetValue<string>("SmtpServer"), emailConfig.GetValue<int>("Port"), SecureSocketOptions.SslOnConnect);
                    smtp.Authenticate(emailConfig.GetValue<string>("Username"), emailConfig.GetValue<string>("Password"));
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(Patient.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Home");
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
    }
}

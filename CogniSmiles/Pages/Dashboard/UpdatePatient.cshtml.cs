using CogniSmiles.Data;
using CogniSmiles.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using NuGet.Protocol.Plugins;
using CogniSmiles.Interfaces;

namespace CogniSmiles.Pages.Dashboard
{
    public class UpdatePatientModel : AuthModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IEmailService _emailService;
        private PatientStatus _patientStatus;
        public UpdatePatientModel(CogniSmilesContext context, IEmailService emailService)
        {
            _context = context;  
            _emailService = emailService;
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
                    
                    var adminId = _context.Login.Where(l => l.AuthType == AuthType.Admin).Select(d => d.DoctorId).FirstOrDefault();
                    var docEmail = _context.Doctor.Where(d1 => d1.Id == adminId).Select(d => d.Email).FirstOrDefault();

                    if (IsAdmin && Patient.PatientStatus == (PatientStatus.AmendPlan | PatientStatus.ApprovePlan))
                    {
                        //Get Doctor Email
                        docEmail = _context.Doctor.Where(d1 => d1.Id == Patient.DoctorId).Select(d => d.Email).FirstOrDefault(); ;                       
                    }

                    var config = new Dictionary<string, object>()
                    {
                        {"ToEmail",docEmail },
                        {"PatientID", Patient.Id}
                    };

                    _emailService.SendEmail(Services.EmailType.Notification, config);
                    
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

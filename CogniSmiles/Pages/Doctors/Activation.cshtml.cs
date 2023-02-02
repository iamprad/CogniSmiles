using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;
using CogniSmiles.Interfaces;

namespace CogniSmiles.Pages.Doctors
{
    public class ActivationModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IEmailService _emailService;
        public ActivationModel(CogniSmilesContext context, IEmailService emailService)
        {
            _context = context;      
            _emailService = emailService;
        }

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
            _emailService.SendEmail(Services.EmailType.Activation, login.UserId.ToString(), DoctorEmail);

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

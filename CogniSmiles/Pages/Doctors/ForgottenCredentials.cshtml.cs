using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;
using CogniSmiles.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CogniSmiles.Pages.Doctors
{
    public class ForgottenCredentialsModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IEmailService _emailService;
        public ForgottenCredentialsModel(CogniSmilesContext context, IEmailService emailService)
        {
            _context = context;      
            _emailService = emailService;
        }
        public string Action;
        [BindProperty]
        [Display(Name = "Email Addresss")]
        public string EmailAddress { get; set; }
        public string ErrorMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string action)
        {
            Action = action;

            if (Action == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            ErrorMessage = string.Empty;
            Action = action;
            if (Action == null)
                return NotFound();
            var doctor = await _context.Doctor.FirstOrDefaultAsync(d1 => d1.Email == EmailAddress);
            if (doctor != null)
            {
                var login = await _context.Login.FirstOrDefaultAsync(l1 => l1.DoctorId == doctor.Id);
                if (login != null)
                {
                    var userId = login.UserId;
                    bool emailSent = false;
                    if(action == "username")
                        emailSent = _emailService.SendEmail(Services.EmailType.ForogttenUserName, login.UserId.ToString(), doctor.Email,null,login.UserName);
                    else if(action == "password")
                        emailSent = _emailService.SendEmail(Services.EmailType.ForogttenPassword, login.UserId.ToString(), doctor.Email, null, login.UserName);
                    if(emailSent)
                    {
                        ErrorMessage = "Email has been sent to your registered email address";
                    }
                    else
                        ErrorMessage = "Failed to send Email. Contact Site Administrator";
                }
            }
            else
                ErrorMessage = "Email not registered with us";

            
            return Page();
        }


    }
}

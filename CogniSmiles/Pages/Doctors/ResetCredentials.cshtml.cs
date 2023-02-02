using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;
using CogniSmiles.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CogniSmiles.Pages.Doctors
{
    public class ResetCredentialsModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        public ResetCredentialsModel(CogniSmilesContext context)
        {
            _context = context;     
        }
        public string UserID;

        [BindProperty]
        [Display(Name ="Email Addresss")]
        public string EmailAddress { get; set; }
        [BindProperty]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [BindProperty]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValidRequest { get; set; }
        public bool IsResetSuccess { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            UserID = userId;
            IsValidRequest = false; 
            if (UserID != null)
            {
                var login = await _context.Login.FirstOrDefaultAsync(l1 => l1.UserId == Guid.Parse(UserID));
                if (login != null)
                {
                    var doctor = await _context.Doctor.FirstOrDefaultAsync(d1 => d1.Id == login.DoctorId);
                    if (doctor != null)
                    {
                        EmailAddress = doctor.Email;
                        IsValidRequest = true;
                    }
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ErrorMessage = string.Empty;
            IsResetSuccess = false;
            if (NewPassword == ConfirmPassword)
            {
                var doctor = await _context.Doctor.FirstOrDefaultAsync(d1 => d1.Email == EmailAddress);
                if (doctor != null)
                {
                    var login = await _context.Login.FirstOrDefaultAsync(l1 => l1.DoctorId == doctor.Id);
                    if (login != null)
                    {
                        login.Password = NewPassword;
                        login.UpdatedOn= DateTime.Now;                        
                        _context.Login.Update(login);
                        _context.SaveChanges();
                        IsResetSuccess = true;
                    }
                }
                else
                    ErrorMessage = "Email not registered with us";
            }
            else
                ErrorMessage = "New Password does not match with Confirm Password. Try Again!!";
            
            return Page();
        }


    }
}

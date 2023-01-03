using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CogniSmiles.Data;
using CogniSmiles.Models;

namespace CogniSmiles.Pages.Doctors
{
    public class LoginModel : AuthModel
    {
        private readonly CogniSmilesContext _context;
        //private AuthModel _authModel;

        public LoginModel(CogniSmilesContext context)
        {
            _context = context;
            
        }

        public IActionResult OnGet()
        {
            // _authModel = new AuthModel(HttpContext.Session);
            if (IsAuthenticated)
                return RedirectToPage("../Dashboard/Home");   
            return Page();
        }

        [BindProperty]
        public Login DoctorLogin { get; set; }       


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //_authModel = new AuthModel(HttpContext.Session);
            if (!ModelState.IsValid)
            {
                return Page();
            }            
            
            var login = _context.Login.Where(login => login.UserName == DoctorLogin.UserName && login.Password == DoctorLogin.Password).SingleOrDefault();
            ViewData["LoginActive"] = true; 
            if (login == null)
            {
                ViewData["LoginError"] = "Invalid Login Credentials";
                return Page();
            }
                
            else if (login.IsLocked)
            {
                ViewData["LoginError"] = "Your Account is Locked.";
                return Page();
            }
                
            else if (!login.IsActive)
            {
                DoctorLogin.DoctorId = login.DoctorId;
                ViewData["LoginError"] = "Your Account is not activated. Please activate your account before logging in.";
                ViewData["LoginActive"] = false;
                return Page();
            }
            
            login.LastLogin = DateTime.Now;
            
            _context.Login.Update(login);
            await _context.SaveChangesAsync();

            IsAuthenticated = true;
            IsAdmin = (login.AuthType == AuthType.Admin);

            DoctorId = login.DoctorId;

            return RedirectToPage("../Dashboard/Home");
        }
    }
}

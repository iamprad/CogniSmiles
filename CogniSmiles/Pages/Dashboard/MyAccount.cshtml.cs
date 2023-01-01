using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Doctors
{
    public class MyAccountModel : AuthModel
    {
        private readonly CogniSmilesContext _context;

        [BindProperty]
        public Login DoctorLogin { get; set; }

        [BindProperty]
        public Doctor DoctorDetails { get; set; }
        public MyAccountModel(CogniSmilesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAuthenticated)
                return RedirectToPage("../Index");

            else if (IsAuthenticated && DoctorId > 0)
            {
                
                var doc = await _context.Doctor.Include(a => a.BillingAddress).Include(b => b.DeliveryAddress).Where(d => d.Id == DoctorId).FirstOrDefaultAsync();
                if (doc != null)
                { 
                    DoctorDetails = doc;
                    var login = await _context.Login.Where(d => d.DoctorId == DoctorId).FirstOrDefaultAsync();
                    if (login != null)
                        DoctorLogin = login;
                    else
                        return RedirectToPage("../Index");
                }
                else
                    return RedirectToPage("../Index");
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(DoctorDetails).State = EntityState.Modified;
            DoctorDetails.UpdatedDate = DateTime.Now;

            var login = await _context.Login.Where(l => l.DoctorId == DoctorId).FirstOrDefaultAsync();
            if(login != null && !string.IsNullOrEmpty(DoctorLogin.UserName) && !string.IsNullOrEmpty(DoctorLogin.Password))
            {
                bool loginUpdated = false;
                if (login.UserName != DoctorLogin.UserName)
                { 
                    login.UserName = DoctorLogin.UserName;
                    loginUpdated = true;
                }
                if (login.Password != DoctorLogin.Password)
                { 
                    login.Password = DoctorLogin.Password;
                    loginUpdated = true;
                }
                if(loginUpdated)
                {
                    login.UpdatedOn = DateTime.Now;
                    _context.Login.Update(login);
                }                
            }           

            await _context.SaveChangesAsync();

            return RedirectToPage("./Home");
        }
    }
}

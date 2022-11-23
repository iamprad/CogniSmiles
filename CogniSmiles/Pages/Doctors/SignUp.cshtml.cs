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
    public class SignUpModel : PageModel
    {
        private readonly CogniSmilesContext _context;

        public SignUpModel(CogniSmilesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Login DoctorLogin { get; set; }

        [BindProperty]
        public Doctor DoctorDetails { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Doctor.Add(DoctorDetails);

            await _context.SaveChangesAsync();

            DoctorLogin.DoctorId = DoctorDetails.Id;

            _context.Login.Add(DoctorLogin);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Activation",new { id= DoctorDetails.Id});
        }
    }
}

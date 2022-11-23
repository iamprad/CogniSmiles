using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;

namespace CogniSmiles.Pages.Doctors
{
    public class DetailsModel : PageModel
    {
        private readonly CogniSmiles.Data.CogniSmilesContext _context;

        public DetailsModel(CogniSmiles.Data.CogniSmilesContext context)
        {
            _context = context;
        }

      public Doctor Doctor { get; set; }

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
            else 
            {
                Doctor = doctor;
            }
            return Page();
        }
    }
}

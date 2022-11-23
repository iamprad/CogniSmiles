using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Models;

namespace CogniSmiles.Pages.Dashboard
{
    public class PatientDetailsModel : PageModel
    {
        private readonly Data.CogniSmilesContext _context;

        public PatientDetailsModel(Data.CogniSmilesContext context)
        {
            _context = context;
        }

      public Patient Patient { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Patient == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            else 
            {
                Patient = patient;
            }
            return Page();
        }
    }
}

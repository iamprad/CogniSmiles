using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CogniSmiles.Data;
using CogniSmiles.Models;

namespace CogniSmiles.Pages.Dashboard
{
    public class NewPatientModel : AuthModel
    {
        private readonly CogniSmilesContext _context;       
        public NewPatientModel(CogniSmilesContext context)
        {
            _context = context;            
        }

        public IActionResult OnGet()
        {
            // Authorize           
            if (!IsAuthenticated)
                return NotFound();
            return Page();
        }

        [BindProperty]
        public Patient Patient { get; set; }        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Authorize            
            if (!ModelState.IsValid)
            {
                return Page();
            }            
            Patient.DoctorId = DoctorId;
            _context.Patient.Add(Patient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Home");
        }
    }
}

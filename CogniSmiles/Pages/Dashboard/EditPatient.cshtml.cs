using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Models;

namespace CogniSmiles.Pages.Dashboard
{
    public class EditPatient : AuthModel
    {
        private readonly CogniSmilesContext _context;

        public EditPatient(CogniSmilesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (!IsAuthenticated && (id == null || _context.Patient == null))
            {
                return NotFound();
            }

            var patient =  await _context.Patient.FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
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

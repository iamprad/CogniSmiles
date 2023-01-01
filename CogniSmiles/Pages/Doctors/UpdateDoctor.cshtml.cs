using Microsoft.AspNetCore.Mvc;
using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Doctors
{
    public class UpdateDoctorModel : AuthModel
    {
        private readonly CogniSmilesContext _context;

        [BindProperty]
        public Doctor DoctorDetails { get; set; }
        public UpdateDoctorModel(CogniSmilesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAuthenticated && !IsAdmin)
                return RedirectToPage("../Index");

            else if (IsAuthenticated && DoctorId > 0)
            {
                
                var doc = await _context.Doctor.Include(a => a.BillingAddress).Include(b => b.DeliveryAddress).Where(d => d.Id == DoctorId).FirstOrDefaultAsync();
                if (doc != null)
                { 
                    DoctorDetails = doc;                   
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
            await _context.SaveChangesAsync();

            return RedirectToPage("./Home");
        }
    }
}

using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Dashboard
{
    public class HomeModel : AuthModel
    {
        private readonly CogniSmilesContext _context;        
        public IList<Patient> Patient { get; set; } = default!;
        public HomeModel(CogniSmilesContext context)
        {           
            _context = context;
            Patient = new List<Patient>();
           
        }
        public async Task<ActionResult> OnGetAsync()
        {            
            if (!IsAuthenticated)
                return RedirectToPage("../Index");
            
            else if (IsAuthenticated && DoctorId > 0)
            {
                Patient = await _context.Patient.Where(p => p.DoctorId == DoctorId).ToListAsync();
            }
            return Page();
        }
        
        public ActionResult OnGetSignOut()
        {
            if (!IsAuthenticated)
                return RedirectToPage("../Index");

            ClearSession();            
            return RedirectToPage("../Index"); 
        }
    }
}

using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Dashboard
{
    public class HomeModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        private AuthModel _authModel;
        public IList<Patient> Patient { get; set; } = default!;
        public HomeModel(CogniSmilesContext context)
        {           
            _context = context;
            Patient = new List<Patient>();
           
        }
        public async Task<ActionResult> OnGetAsync()
        {
            _authModel = new AuthModel(HttpContext.Session);
            if (!_authModel.IsAuthenticated)
                return RedirectToPage("../Index");
            
            else if (_authModel.IsAuthenticated && _authModel.DoctorId > 0)
            {
                Patient = await _context.Patient.Where(p => p.DoctorId == _authModel.DoctorId).ToListAsync();
            }
            return Page();
        }
    }
}

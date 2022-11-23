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
    public class IndexModel : PageModel
    {
        private readonly CogniSmilesContext _context;

        public IndexModel(CogniSmilesContext context)
        {
            _context = context;
        }

        public IList<Doctor> Doctor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Doctor != null)
            {
                Doctor = await _context.Doctor.ToListAsync();
            }
        }
    }
}

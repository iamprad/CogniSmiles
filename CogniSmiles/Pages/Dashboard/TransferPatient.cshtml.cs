using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using CogniSmiles.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace CogniSmiles.Pages.Dashboard
{
    public class TransferPatientModel : AuthModel
    {
        private readonly CogniSmilesContext _context;
        private readonly IEmailService _emailService;
        public TransferPatientModel(CogniSmilesContext context, IEmailService emailService)
        {
            _context = context;  
            _emailService = emailService;
        }
        [BindProperty]
        public int? PatientId { get; set; } = default!;

        public string PracticeName { get; set; } = default!;
        [BindProperty]
        public string PatientCode { get; set; } = default!;
        [BindProperty]
        public string NewPractice { get; set; } = default!;
        
        public Dictionary<int,string> PracticeList { get; set; }

        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!IsAuthenticated && (id == null || _context == null))
            {
                return NotFound();
            }

            var patient = _context.Patient.Where(d1 => d1.Id == id).FirstOrDefault();
            if (patient == null || _context.Doctor == null)
            {
                return BadRequest();
            }
            PatientId = id;
            PatientCode = patient.PatientCode!;
#pragma warning disable CS8601 // Possible null reference assignment.
            PracticeName = _context.Doctor.Where(d1 => d1.Id == patient.DoctorId).Select(d => d.PracticeName).FirstOrDefault();

            if (PracticeName == null)
            {
                return BadRequest();
            }
            NewPractice = _context.Doctor.Where(d1 => d1.Id == DoctorId).Select(d => d.PracticeName).FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.

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
            
            var patient = _context.Patient.Where(d1 => d1.Id == PatientId).FirstOrDefault();
            if (patient == null)
            {
                return BadRequest();
            }
            patient.DoctorId = DoctorId;

            _context.Patient.Update(patient);
            return RedirectToPage("./Home");
        }

    }
}

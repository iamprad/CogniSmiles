using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CogniSmiles.Pages.Dashboard
{
    public class HomeModel : AuthModel
    {
        private readonly CogniSmilesContext _context;
        public IList<PatientData> PatientList { get; set; } = default!;
        [BindProperty]
       
        public string PatientSearchTerm { get; set; }
        public HomeModel(CogniSmilesContext context)
        {
            _context = context;
            PatientList = new List<PatientData>();

        }
        public async Task<ActionResult> OnGetAsync()
        {
            if (!IsAuthenticated)
                return RedirectToPage("../Index");

            else if (IsAuthenticated && DoctorId > 0)
            {
                PopulatePatientList();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {            
            PopulatePatientList();
            return Page();
        }
        public ActionResult OnGetSignOut()
        {
            if (!IsAuthenticated)
                return RedirectToPage("../Index");

            ClearSession();
            return RedirectToPage("../Index");
        }
        private void PopulatePatientList()
        {
            var patientData = from patient in _context.Set<Patient>()
                              join doctor in _context.Set<Doctor>()
                                  on patient.DoctorId equals doctor.Id
                              select new PatientData
                              {
                                  Id = patient.Id,
                                  DoctorId = patient.DoctorId,
                                  PracticeName = doctor.PracticeName,
                                  DentistName = doctor.FullName,
                                  PatientCode = patient.PatientCode,
                                  SurgicalGuideReturnDate = patient.SurgicalGuideReturnDate,
                                  ImplantSite = patient.ImplantSite,
                                  ImplantSystem = patient.ImplantSystem,
                                  ImplantDiameter = patient.ImplantDiameter,
                                  ImplantLength = patient.ImplantLength,
                                  PatientStatus = patient.PatientStatus
                              };
            if(!string.IsNullOrEmpty(PatientSearchTerm))
            {
                patientData = patientData.Where( p => p.PatientCode.Contains(PatientSearchTerm) || p.PracticeName.Contains(PatientSearchTerm) || p.DentistName.Contains(PatientSearchTerm));
            }
            if (IsAdmin)
            {
                PatientList = patientData.ToList();
            }
            else
            {
                PatientList = patientData.Where(p => p.DoctorId == DoctorId).ToList();
            }
        }
    }
    public class PatientData : Patient
    {
        [Display(Name = "Practice Name")]
        public string PracticeName { get; set; }
        [Display(Name ="Dentist Name")]
        public string DentistName { get; set; }
    }
   
}

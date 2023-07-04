using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CogniSmiles.Pages.Dashboard
{
    public class HomeModel : AuthModel
    {
        private readonly CogniSmilesContext _context;

        [BindProperty]
        public List<PatientData> PatientList { get; set; }

        [BindProperty]
       
        public string PatientSearchTerm { get; set; }
        public HomeModel(CogniSmilesContext context)
        {
            _context = context;
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
        public async Task<IActionResult> OnPostSearchAsync()
        {            
            PopulatePatientList();
            return Page();
        }
        public async Task<IActionResult> OnPostArchiveAsync()
        {
            foreach (var pt in PatientList.Where(p => p.Selected == true))
            {
                var patient = _context.Patient.Where(p => p.Id == pt.Id).FirstOrDefault();
                if (patient != null)
                {
                    patient.IsArchived = true;
                    _context.Patient.Update(patient); 
                    _context.SaveChanges();
                }
            }
            PopulatePatientList();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            foreach (var pt in PatientList.Where(p => p.Selected == true))
            {
                var patient = _context.Patient.Where(p => p.Id == pt.Id).FirstOrDefault();
                if (patient != null)
                {
                    // delete doctor comments on patient (if any)
                    var dcList = _context.DoctorComment.Where(dc => dc.PatientId == pt.Id).ToList();
                    if(dcList.Any())
                    {
                        _context.DoctorComment.RemoveRange(dcList.ToArray());
                    }

                    // delete patient file records related to patient (if any)
                    var ptFiles = _context.PatientFile.Where(dc => dc.PatientId == pt.Id).ToList();
                    if (ptFiles.Any())
                    {
                        // Delete files permanently from the system folder
                        foreach( var file in ptFiles.Where(pf => pf.FilePath != null))
                        {
                            if (System.IO.File.Exists(file.FilePath))
                                System.IO.File.Delete(file.FilePath);
                        }
                        
                        _context.PatientFile.RemoveRange(ptFiles.ToArray());
                    }

                    _context.Patient.Remove(patient);
                    _context.SaveChanges();
                }
            }
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
        private void PopulatePatientList(int limit = 10)
        {

            var patientData = (from patient in _context.Set<Patient>().Where(p => p.IsArchived == false)
                               join doctor in _context.Set<Doctor>()
                                   on patient.DoctorId equals doctor.Id 
                               orderby patient.Id descending
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
                                   PatientStatus = patient.PatientStatus,
                                   Selected = false
                               }).Take(limit);
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
        public bool Selected { get; set; }
    }
   
}

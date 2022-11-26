using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Migrations;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Dashboard
{
    public class UploadFilesModel : AuthModel
    {
        private readonly CogniSmilesContext _context;        
        private readonly IFileUploadService _fileUploadService;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public IList<PatientFile> PatientFiless { get; set; }
        public UploadFilesModel(CogniSmilesContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            PatientFiless = new List<PatientFile>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Authorize             
            if (!IsAuthenticated)
                return NotFound();
           
            PatientFiless = await _context.PatientFile.Where(file => file.PatientId == Id).ToListAsync();
            return Page();
        }
        

        [BindProperty]
        public PatientFile NewPatientFile { get; set; }
        
        [BindProperty]
        public IFormFile Upload { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Authorize            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (await _fileUploadService.UploadFile(Upload))
                {
                    ViewData["UploadStatus"] = "File Upload Successful";
                    NewPatientFile.PatientId = Id;
                    NewPatientFile.FileName = Upload.FileName;
                    NewPatientFile.FilePath = _fileUploadService.FilePath;
                    NewPatientFile.DateUploaded = DateTime.Now;
                    _context.PatientFile.Add(NewPatientFile);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["UploadStatus"] = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                //Log ex
                ViewData["UploadStatus"] = "File Upload Failed";
            }
            return Page();
        }
    }
}

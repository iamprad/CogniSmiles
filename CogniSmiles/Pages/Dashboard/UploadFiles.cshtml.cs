using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniSmiles.Pages.Dashboard
{
    public class UploadFilesModel : PageModel
    {
        private readonly CogniSmilesContext _context;
        private AuthModel _authModel;
        private readonly IFileUploadService _fileUploadService;
        public UploadFilesModel(CogniSmilesContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public IActionResult OnGet()
        {
            // Authorize 
            _authModel = new AuthModel(HttpContext.Session);
            if (!_authModel.IsAuthenticated)
                return NotFound();
            return Page();
        }

        [BindProperty]
        public PatientFile PatientFile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            // Authorize 
            _authModel = new AuthModel(HttpContext.Session);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //PatientFile.PatientId = _authModel.DoctorId;
            //_context.PatientFile.Add(PatientFile);
            //await _context.SaveChangesAsync();
            try
            {
                if (await _fileUploadService.UploadFile(file))
                {
                    ViewData["UploadStatus"] = "File Upload Successful";
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

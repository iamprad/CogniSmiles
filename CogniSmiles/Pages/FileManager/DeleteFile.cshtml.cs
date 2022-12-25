using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;

namespace CogniSmiles.Pages.FileManager
{
    public class DeleteFileModel : AuthModel
    {
        [BindProperty]
        public PatientFile DeletionFile { get; set; }

        private readonly CogniSmilesContext _context;

        public DeleteFileModel(CogniSmilesContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int id)
        {
            if (!IsAuthenticated)
                return Page();
           
            var file = _context.PatientFile.Where(x => x.Id == id).FirstOrDefault();
            if(file == null)
                return Page();

            DeletionFile = file;

            // Build the File Path.
            string path = Path.Combine(DeletionFile.FilePath, DeletionFile.FileName);

            //delete the File.
            System.IO.File.Delete(path);
            // remove file record
            _context.PatientFile.Remove(DeletionFile);
            _context.SaveChanges();
            
            return Page();
            
        }       
        public IActionResult OnPost()
        {
            string path = Path.Combine(DeletionFile.FilePath, DeletionFile.FileName);

            //delete the File.
            System.IO.File.Delete(path);
            // remove file record
            _context.PatientFile.Remove(DeletionFile);
            _context.SaveChanges();

            return Page();
        }
    }
}
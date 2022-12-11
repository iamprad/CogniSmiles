using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;

namespace CogniSmiles.Pages.FileManager
{
    public class DownloadFileModel : AuthModel
    {
        [BindProperty]
        public PatientFile DownloadFile { get; set; }

        private readonly CogniSmilesContext _context;

        public DownloadFileModel(CogniSmilesContext context)
        {
            _context = context;
        }
        public FileResult? OnGet(int id)
        {
            if (!IsAuthenticated)
                return null;
           
            var file = _context.PatientFile.Where(x => x.Id == id).FirstOrDefault();
            if(file == null)
                return null;

            DownloadFile = file;

            // Build the File Path.
            string path = Path.Combine(DownloadFile.FilePath,DownloadFile.FileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", DownloadFile.FileName);
            
        }       
        public FileResult? OnPostDownload()
        {
            // Build the File Path.
            string path = Path.Combine(DownloadFile.FilePath, DownloadFile.FileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", DownloadFile.FileName);
        }
    }
}

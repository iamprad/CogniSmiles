using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CogniSmiles.Pages.FileManager
{
    public class ManageFilesModel : AuthModel
    {
        private readonly CogniSmilesContext _context;        
        private readonly IFileUploadService _fileUploadService;
        private readonly IEmailService _emailService;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public IList<PatientFile> PatientFiless { get; set; }
        [BindProperty]
        [Display(Name = "Patient Status")]
        public PatientStatus PatientStatus { get; set; }

        public DoctorComment NewComment { get; set; }
        [BindProperty]
        public string PostedComment { get; set; }

        public string PatientName { get; set; }

        public ManageFilesModel(CogniSmilesContext context, IFileUploadService fileUploadService, IEmailService emailService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            PatientFiless = new List<PatientFile>();
            _emailService = emailService;
            NewComment = new DoctorComment();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Authorize             
            if (!IsAuthenticated)
                return RedirectToPage("../Doctors/Login");

            PatientName = await _context.Patient.Where(p1 => p1.Id == Id).Select(p => p.PatientCode).FirstOrDefaultAsync();
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
                    ViewData["UploadStatus"] = "File Upload Successful. ";
                    // Add new file
                    NewPatientFile.PatientId = Id;
                    NewPatientFile.FileName = Upload.FileName;
                    NewPatientFile.FilePath = _fileUploadService.FilePath;
                    NewPatientFile.DateUploaded = DateTime.Now;
                    _context.PatientFile.Add(NewPatientFile);

                    // Add new Comment
                    NewComment.CommentDate = DateTime.Now;
                    NewComment.PatientId = Id;
                    NewComment.DoctorId = DoctorId;
                    NewComment.Comment = PostedComment;
                    _context.DoctorComment.Add(NewComment);                    
                    

                    // Update patient status
                    var patient = _context.Patient.Where(p => p.Id== Id).FirstOrDefault();
                    if (patient != null)
                    {
                        patient.PatientStatus = PatientStatus;
                        _context.Patient.Update(patient);
                    }
                    await _context.SaveChangesAsync();

                    ViewData["UploadStatus"] += "Dentist Comments Updated. ";

                    // Send email notification to Aditya (Admin)
                    var adminId = _context.Login.Where(l => l.AuthType == AuthType.Admin).Select(d => d.DoctorId).FirstOrDefault();
                    var docEmail = _context.Doctor.Where(d1 => d1.Id == adminId).Select(d => d.Email).FirstOrDefault();

                    if (IsAdmin)
                    {
                        //Get Doctor Email
                        docEmail = _context.Doctor.Where(d1 => d1.Id == DoctorId).Select(d => d.Email).FirstOrDefault(); ;
                    }
                    var config = new Dictionary<string, object>()
                    {
                        {"ToEmail",docEmail },
                        {"PatientID", Id},
                        {"DoctorID",DoctorId }
                    };

                    if (_emailService.SendEmail(Services.EmailType.Notification, config))
                    {
                        ViewData["UploadStatus"] += "Notification Sent via email";
                    }
                    else
                        ViewData["UploadStatus"] += "Notification failed to Send to Admin. Contact them to notify";
                }
                else
                {
                    ViewData["UploadStatus"] = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                //Log ex
                ViewData["UploadStatus"] = "File Upload Failed. Error:" + ex.Message;
            }
            PatientFiless = await _context.PatientFile.Where(file => file.PatientId == Id).ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAddCommentAsync()
        {
            if (PostedComment == null)
                return Page();

            NewComment.CommentDate = DateTime.Now;
            NewComment.PatientId = Id;
            NewComment.DoctorId = DoctorId;
            NewComment.Comment = PostedComment;
            _context.DoctorComment.Add(NewComment);

            await _context.SaveChangesAsync();

            return Page();
        }
    }
}

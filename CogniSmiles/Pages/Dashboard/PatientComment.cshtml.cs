using CogniSmiles.Data;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CogniSmiles.Pages.Dashboard
{
    public class PatientCommentModel : AuthModel
    {
        private readonly CogniSmilesContext _context;

        public PatientCommentModel(CogniSmilesContext context)
        {
            _context = context;
            DoctorComments = new List<DoctorComment>();
            NewComment = new DoctorComment();
        }
        
        [BindProperty]
        public string PostedComment { get; set; }
        public IList<DoctorComment> DoctorComments { get; set; }
        public DoctorComment NewComment { get; set; }
        public int? PatientId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!IsAuthenticated && (id == null || _context.Patient == null))
            {
                return NotFound();
            }

            var patient = await _context.Patient.FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            PatientId = id;
            var comments = await _context.DoctorComment.Where(m => m.PatientId == id && m.DoctorId == DoctorId).ToListAsync();
            if (comments != null)
            {
                DoctorComments = comments;
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.      
        public async Task<IActionResult> OnPostAddCommentAsync()
        { 
            if(PostedComment == null || PatientId == null)
                return Page();

            NewComment.CommentDate = DateTime.Now;
            NewComment.PatientId = (int)PatientId;
            NewComment.DoctorId = DoctorId;
            NewComment.Comment = PostedComment;
            _context.DoctorComment.Add(NewComment);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Home");
        }
        public async Task<IActionResult> OnPostDeleteCommentAsync(int CommentId)
        {           
            var comment = await _context.DoctorComment.FirstOrDefaultAsync(c => c.Id == CommentId);

            if (comment != null)
            {
                _context.DoctorComment.Remove(comment);

                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Home");
        }
    }
}

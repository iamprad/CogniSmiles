using CogniSmiles.Data;
using CogniSmiles.Models;
using CogniSmiles.Models.View_Model;
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
            DoctorComments = new List<ViewCommentsModel>();
            NewComment = new DoctorComment();
        }
        
        [BindProperty]
        public string PostedComment { get; set; }
        public IList<ViewCommentsModel> DoctorComments { get; set; }
        public DoctorComment NewComment { get; set; }
        [BindProperty]
        public int? PatientId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!IsAuthenticated && (id == null || _context.Patient == null))
            {
                return NotFound();
            }
            await getComments(id);           

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.      
        public async Task<IActionResult> OnPostAddCommentAsync()
        { 
            if(PostedComment == null || PatientId == null)
                return Page();

            NewComment.CommentDate = DateTime.Now;
            NewComment.PatientId =(int) PatientId;
            NewComment.DoctorId = DoctorId;
            NewComment.Comment = PostedComment;
            _context.DoctorComment.Add(NewComment);

            await _context.SaveChangesAsync();

            await getComments(PatientId);

            return Page();
        }
        private async Task getComments(int? id)
        {
            var patient = await _context.Patient.FirstOrDefaultAsync(m => m.Id == id);
            if (patient != null)
            {
                PatientId = id;
                var commentsNew = from dc in _context.Set<DoctorComment>() 
                                    join d in _context.Set<Doctor>() on dc.DoctorId equals d.Id
                                    where dc.PatientId == PatientId
                                  select new ViewCommentsModel() { CommentDate = dc.CommentDate, PatientId = dc.PatientId, DoctorId = dc.DoctorId, Comment = dc.Comment,PracticeName = d.PracticeName };
                if (commentsNew != null)
                {
                    if (!IsAdmin)
                        commentsNew = commentsNew.Where(dc => dc.DoctorId == DoctorId);
                    DoctorComments = commentsNew.ToList();
                }
            }            
        }
    }
}

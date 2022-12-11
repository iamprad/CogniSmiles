namespace CogniSmiles.Models
{
    public class DoctorComment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }
}

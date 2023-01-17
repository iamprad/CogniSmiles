namespace CogniSmiles.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string? PatientCode { get; set; } 
        public DateTime? SurgicalGuideReturnDate { get; set; }
        public string? ImplantSite { get; set; }
        public string? ImplantSystem { get; set; }
        public int? ImplantDiameter { get; set; }
        public int? InmplantLength { get; set; }
        public string? DicomFile { get; set; }
        public string? StlIosFile { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public string? Comments { get; set; }
    }
    public enum PatientStatus
    {
        NewEntry = 0,
        AwaitingPlanning = 1,
        MoreInfoRequested = 2,
        AwaitingApproval = 3,
        AmendPlan = 4,
        ApprovePlan = 5,
        GuidePosted = 6
    }
}

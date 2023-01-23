using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CogniSmiles.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        [Display(Name = "Patient Code")]
        public string? PatientCode { get; set; }
        [Display(Name = "Surgical Guide Return Date")]
        public DateTime? SurgicalGuideReturnDate { get; set; }
        [Display(Name = "Implant Site")]
        public string? ImplantSite { get; set; }
        [Display(Name = "Implant System")]
        public string? ImplantSystem { get; set; }
        [Display(Name = "Implant Diameter")]
        public int? ImplantDiameter { get; set; }
        [Display(Name = "Implant Length")]
        public int? ImplantLength { get; set; }
        public string? DicomFile { get; set; }
        public string? StlIosFile { get; set; }
        [Display(Name = "Patient Status")]
        public PatientStatus PatientStatus { get; set; }
        public string? Comments { get; set; }
    }
    public enum PatientStatus
    {
        [Display(Name = "New Entry")]
        NewEntry = 0,
        [Display(Name = "Awaiting Planning")]
        AwaitingPlanning = 1,
        [Display(Name = "More Info Requested")]
        MoreInfoRequested = 2,
        [Display(Name = "Awaiting Approval")]
        AwaitingApproval = 3,
        [Display(Name = "Amend Plan")]
        AmendPlan = 4,
        [Display(Name = "Approve Plan")]
        ApprovePlan = 5,
        [Display(Name = "Guide Posted")]
        GuidePosted = 6
    }
}

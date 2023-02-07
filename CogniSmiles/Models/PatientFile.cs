using System.ComponentModel.DataAnnotations;

namespace CogniSmiles.Models
{
    public class PatientFile
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [Display(Name ="File Name")]
        public string? FileName { get; set; }
        [Display(Name = "File Type")]
        public FileType FileType { get; set; }
        public string? FilePath { get; set; }
        public DateTime? ScanTakenDate { get; set; }
        public DateTime DateUploaded { get; set; }
    }
    public enum FileType
    {
        DICOMFile = 0,
        STLFile = 1,
        Plan = 2
    }
}

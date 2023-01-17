namespace CogniSmiles.Models
{
    public class PatientFile
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? FileName { get; set; } 
        public FileType FileType { get; set; }
        public string? FilePath { get; set; }
        public DateTime DateUploaded { get; set; }
    }
    public enum FileType
    {
        DICOMFile = 0,
        STLFile = 1
    }
}

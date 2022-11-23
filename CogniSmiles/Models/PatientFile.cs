namespace CogniSmiles.Models
{
    public class PatientFile
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? FileName { get; set; } 
        public FileType FileType { get; set; }
        public string? FilePath { get; set; }
        public string? DateUploaded { get; set; }
    }
    public enum FileType
    {
        DicomFile = 0,
        StlIosFile = 1
    }
}

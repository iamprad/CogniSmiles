using Microsoft.AspNetCore.WebUtilities;

namespace CogniSmiles.Interfaces
{
    public interface IFileUploadService
    {
        string? FilePath { get; set; }
        Task<bool> UploadFile(IFormFile file);
    }
}


using Microsoft.AspNetCore.WebUtilities;

namespace CogniSmiles.Interfaces
{
    public interface IFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
    }
}


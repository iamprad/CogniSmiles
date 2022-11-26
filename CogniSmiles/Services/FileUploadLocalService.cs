using CogniSmiles.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace CogniSmiles.Services
{
    public class FileUploadLocalService : IFileUploadService
    {
        public string? FilePath { get; set; }
        public async Task<bool> UploadFile(IFormFile file)
        {
            
            try
            {
                if (file.Length > 0)
                {
                    FilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(FilePath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
    }
}

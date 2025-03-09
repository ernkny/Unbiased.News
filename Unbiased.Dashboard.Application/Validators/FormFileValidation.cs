using Microsoft.AspNetCore.Http;

namespace Unbiased.Dashboard.Application.Validators
{
    public class FormFileValidation
    {
        public bool IsValidFile(IFormFile file)
        {
            
            long maxFileSize = 10 * 1024 * 1024; // 10 MB
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

            if (file == null ||  file.Length == 0 || file.Length > maxFileSize)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !Array.Exists(permittedExtensions, e => e == extension))
            {
                return false;
            }

            return true;
        }
    }
}

using Microsoft.AspNetCore.Http;

namespace Unbiased.Dashboard.Common.Concrete.Helpers
{
    public class FileConvertToByteArray
    {
        public async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }
    }
}


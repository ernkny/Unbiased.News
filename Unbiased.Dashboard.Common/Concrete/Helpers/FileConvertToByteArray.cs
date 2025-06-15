using Microsoft.AspNetCore.Http;

namespace Unbiased.Dashboard.Common.Concrete.Helpers
{
    /// <summary>
    ///  FileConvertToByteArray class is responsible for converting an IFormFile to a byte array.
    /// </summary>
    public class FileConvertToByteArray
    {
        /// <summary>
        ///  Converts an IFormFile to a byte array asynchronously.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return Array.Empty<byte>();
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}


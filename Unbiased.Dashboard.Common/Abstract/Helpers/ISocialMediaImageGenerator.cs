namespace Unbiased.Dashboard.Common.Abstract.Helpers
{
    /// <summary>
    ///  Interface for generating social media images from a URL.
    /// </summary>
    public interface ISocialMediaImageGenerator
    {
        /// <summary>
        ///  Generates a social media image from the specified URL, title, and logo path.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="title"></param>
        /// <param name="logoPath"></param>
        /// <returns></returns>
        Task<byte[]> GenerateFromUrlAsync(string imageUrl, string title, string logoPath);
    }
}

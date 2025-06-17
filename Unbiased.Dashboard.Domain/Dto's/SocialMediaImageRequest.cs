namespace Unbiased.Dashboard.Domain.Dto_s
{
    /// <summary>
    ///  Represents a request for generating a social media image.
    /// </summary>
    public class SocialMediaImageRequest
    {
        /// <summary>
        /// Gets or sets the URL of the image to be used in the social media image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///  Gets or sets the path to the logo to be included in the social media image.
        /// </summary>
        public string Title { get; set; }
    }
}

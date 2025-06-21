using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    /// <summary>
    /// A utility class for generating an image banner with a title and category name.
    /// </summary>
    public static class GenerateImageBannerWithTitle
    {

        /// <summary>
        /// Applies text on an image asynchronously.
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="categoryName"></param>
        /// <param name="title"></param>
        /// <param name="garetFontPath"></param>
        /// <param name="sansFontPath"></param>
        /// <returns></returns>
        public static async Task<byte[]> ApplyTextOnImageAsync(Stream imageStream, string categoryName, string title, string garetFontPath, string sansFontPath)
        {
            try
            {
                using var image = await Image.LoadAsync<Rgba32>(imageStream);

                var fontCollection = new FontCollection();
                var garetFontFamily = fontCollection.Add(garetFontPath);
                var sansFontFamily = fontCollection.Add(sansFontPath);

                var categoryFont = garetFontFamily.CreateFont(116, FontStyle.Bold);
                var titleFont = sansFontFamily.CreateFont(56, FontStyle.Regular);

                var whiteBrush = Brushes.Solid(Color.White);

                float categoryY = image.Height * 0.25f;
                float maxTitleWidth = image.Width * 0.9f;

                var categoryOptions = new TextOptions(categoryFont)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Origin = new PointF(image.Width / 2f, categoryY)
                };

                var titleOptions = new TextOptions(titleFont)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Origin = new PointF(image.Width / 2f, categoryY + 130),
                    WrappingLength = maxTitleWidth,
                    LineSpacing = 1.1f
                };

                image.Mutate(ctx =>
                {
                    ctx.DrawText(categoryOptions, categoryName, whiteBrush);
                    ctx.DrawText(titleOptions, title, whiteBrush);
                });

                using var ms = new MemoryStream();
                await image.SaveAsPngAsync(ms);
                return ms.ToArray();
            }
            catch (Exception exception)
            {

                throw exception;
            }
           
        }
    }
}

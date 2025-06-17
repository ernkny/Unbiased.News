using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Net.Http;
using Unbiased.Dashboard.Common.Abstract.Helpers;

/// <summary>
///  Class for generating social media images from a URL, adding a title and a logo.
/// </summary>
public class SocialMediaImageGenerator : ISocialMediaImageGenerator
{
    private readonly HttpClient _httpClient;
    private readonly Font _font;

    /// <summary>
    ///  Initializes a new instance of the SocialMediaImageGenerator class with the specified HttpClient.
    /// </summary>
    /// <param name="httpClient"></param>
    public SocialMediaImageGenerator(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var fontFamily = SystemFonts.Get("Arial");
        _font = new Font(fontFamily, 48, FontStyle.Bold);
    }

    /// <summary>
    /// Generates a social media image from a URL, adds a title and a logo, and returns the image as a byte array.
    /// </summary>
    /// <param name="imageUrl"></param>
    /// <param name="title"></param>
    /// <param name="logoPath"></param>
    /// <returns></returns>
    public async Task<byte[]> GenerateFromUrlAsync(string imageUrl, string title,string logoPath)
    {
        var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
        using var imageStream = new MemoryStream(imageBytes);
        using Image<Rgba32> logo = Image.Load<Rgba32>(logoPath);
        using var image = await Image.LoadAsync<Rgba32>(imageStream);

        ProcessImage(image, logo, title);

        using var outputStream = new MemoryStream();
        await image.SaveAsJpegAsync(outputStream);
        return outputStream.ToArray();
    }

    /// <summary>
    /// Processes the image by resizing, cropping, adding a logo, and drawing text.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="logo"></param>
    /// <param name="title"></param>
    private void ProcessImage(Image<Rgba32> image, Image<Rgba32> logo, string title)
    {
        int targetWidth = 1080, targetHeight = 1080;

        if (image.Width < targetWidth || image.Height < targetHeight)
        {
            float scale = Math.Max((float)targetWidth / image.Width, (float)targetHeight / image.Height);
            image.Mutate(x => x.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
        }

        var crop = new Rectangle(
            (image.Width - targetWidth) / 2,
            (image.Height - targetHeight) / 2,
            targetWidth,
            targetHeight);
        image.Mutate(x => x.Crop(crop));

        logo.Mutate(x => x.Resize(250, 200));
        var logoPos = new Point(image.Width - logo.Width - 20, 20);
        image.Mutate(x => x.DrawImage(logo, logoPos, 1f));

        int lineWidth = 900, rectHeight = 140, padding = 80;
        var rectX = (image.Width - lineWidth) / 2;
        var rectY = image.Height - rectHeight - padding;
        var rect = new Rectangle(rectX, rectY, lineWidth, rectHeight);
        var center = new PointF(image.Width / 2f, rectY + rectHeight / 2);

        var lineStart = new PointF(rectX, rectY - 10);
        var lineEnd = new PointF(rectX + lineWidth, rectY - 10);
        var drawingOptions = new DrawingOptions
        {
            GraphicsOptions = new GraphicsOptions
            {
                Antialias = true,
                BlendPercentage = 0.6f
            }
        };
        image.Mutate(x =>
        {
            x.DrawLines(Color.Parse("#F9A615"), 5, new[] { lineStart, lineEnd });
            x.Fill(drawingOptions, Color.Black, rect);
            x.DrawText(new TextOptions(_font)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Origin = center,
                WrappingLength = lineWidth,
                TextAlignment = TextAlignment.Center
            }, title, Color.White);
        });
    }
}

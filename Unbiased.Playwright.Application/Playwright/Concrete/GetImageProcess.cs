using Microsoft.Playwright;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Web;

namespace Unbiased.Playwright.Application.Playwright.Concrete
{
    public static class GetImageProcess
    {
        /// <summary>
        /// Gets the images for the news with the given titles
        /// </summary>
        /// <param name="titles"></param>
        /// <param name="watermark"></param>
        /// <returns>List of Base64 images</returns>
        public static async Task<List<string>> GetNewsImageForTitle(List<string> titles, bool watermark = true)
        {
            var imageList = new List<string>();

            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });

            var page = await browser.NewPageAsync();

            foreach (var title in titles)
            {
                await GoToPage(page, $"https://www.google.com/search?q={HttpUtility.UrlEncode(title)}&udm=2");

                imageList.Add(await GetFirstImageBase64(page, watermark));
            }

            await page.CloseAsync();

            return imageList;
        }

        private static async Task<IResponse?> GoToPage(IPage page, string url)
        {
            return await page.GotoAsync(url, new PageGotoOptions
            {
                Timeout = 60000,
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
        }

        private static async Task<string> GetFirstImageBase64(IPage page, bool watermark)
        {
            var imageUrl = await GetImageUrlFromPage(page);

            var imageBytes = await GetImageBytesFromUrl(imageUrl);

            if (watermark)
            {
                imageBytes = GetWatermarkedImage(imageBytes);
            }

            return Convert.ToBase64String(imageBytes);
        }

        private static async Task<string> GetImageUrlFromPage(IPage page)
        {
            const string firstImagePreviewSelector = "div.MjjYud img";
            var imagePreview = await page.QuerySelectorAsync(firstImagePreviewSelector);
            await imagePreview.ClickAsync();

            // TODO: Sometimes wrong url is pulled? Doesn't seem to be related with browser being headless
            // Sleep added to give browser some time to process click event
            Thread.Sleep(1000);

            const string imageSelector = "div.AQyBn a > img";
            await page.WaitForSelectorAsync(imageSelector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            var image = await page.QuerySelectorAsync(imageSelector);

            return await image.GetAttributeAsync("src");
        }

        private static async Task<byte[]> GetImageBytesFromUrl(string url)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        private static byte[] GetWatermarkedImage(byte[] imageBytes)
        {
            using var originalImage = Image.Load(imageBytes);

            originalImage.Mutate(o => DrawWatermark(o));

            using var memoryStream = new MemoryStream();
            originalImage.Save(memoryStream, originalImage.Metadata.DecodedImageFormat);

            return memoryStream.ToArray();
        }

        private static IImageProcessingContext DrawWatermark(IImageProcessingContext context)
        {
            using var watermarkImage = Image.Load("Watermark/watermark.png");
            watermarkImage.Mutate(o => o.Opacity(0.5f).Rotate(-45).Resize(watermarkImage.Width / 2, watermarkImage.Height / 2));

            for (int i = 0; i < context.GetCurrentSize().Width; i += watermarkImage.Bounds.Width + 20)
            {
                for (int j = 0; j < context.GetCurrentSize().Height; j += watermarkImage.Bounds.Height + 20)
                {
                    context = context.DrawImage(watermarkImage, new Point(i, j), 1);
                }
            }

            return context;
        }
    }
}

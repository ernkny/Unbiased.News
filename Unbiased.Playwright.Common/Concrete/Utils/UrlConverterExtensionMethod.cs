using System.Net;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    public static class UrlConverterExtensionMethod
    {
        public async static Task<string> UrlConverterExtension(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            int maxRedirCount = 8;  // prevent infinite loops
            string newUrl = url;
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false  // Don't follow redirects automatically
            };

            using (var client = new HttpClient(handler))
            {
                do
                {
                    try
                    {
                        // Send HEAD request to minimize data usage
                        var request = new HttpRequestMessage(HttpMethod.Head, newUrl);
                        var response = await client.SendAsync(request);

                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                return newUrl;  // Return the URL if it's good
                            case HttpStatusCode.Redirect:
                            case HttpStatusCode.MovedPermanently:
                            case HttpStatusCode.RedirectKeepVerb:
                            case HttpStatusCode.RedirectMethod:
                                var location = response.Headers.Location;
                                if (location == null)
                                    return newUrl;  // If no location header is found, return current URL

                                newUrl = location.IsAbsoluteUri ? location.ToString() : new Uri(new Uri(newUrl), location).ToString();
                                break;
                            default:
                                return newUrl;  // Return the URL for any other status code
                        }
                        url = newUrl;
                    }
                    catch (HttpRequestException)
                    {
                        // Handle HTTP specific exceptions or consider retry logic here
                        return newUrl;  // Return the last known good URL
                    }
                    catch (Exception ex)
                    {
                        // Log exception details here if necessary
                        return null;  // Return null on general error
                    }
                } while (maxRedirCount-- > 0);
            }

            return newUrl;  // Return the last redirected URL
        }
    }
}

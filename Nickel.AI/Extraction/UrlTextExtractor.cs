using PuppeteerSharp;

namespace Nickel.AI.Extraction
{
    public class UrlTextExtractor : ITextExtractor
    {
        /// <summary>
        /// The JavaScript to use for extracting text from the web page. The default
        /// is `document.body.innerText`
        /// </summary>
        public string ScrapeJavascript { get; set; } = "document.body.innerText";

        public ExtractedDocument Extract(Uri uri)
        {
            return Task.Run(() =>
            {
                return Extract(uri.AbsoluteUri, ScrapeJavascript);
            }).GetAwaiter().GetResult();
        }

        private static async Task<ExtractedDocument> Extract(string url, string expression)
        {
            // use Puppeteer so we have a chance at getting content from SPA's or other deferred
            // loading / JS rendered sites.

            var options = new LaunchOptions { Headless = true };

            // this uses headless chrome, download if needed
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(options);
            await using var page = await browser.NewPageAsync();

            await page.GoToAsync(url, new NavigationOptions
            {
                // need to wait for DOM. React will be finicky based on load time but waiting for network idle does help
                WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded, WaitUntilNavigation.Networkidle0 }
            });


            var extractedText = await page.EvaluateExpressionAsync<string>(expression);

            var extractedDocument = new ExtractedDocument();

            extractedDocument.Header = await page.GetTitleAsync();
            extractedDocument.Paragraphs.Add(extractedText);

            return extractedDocument;
        }
    }
}

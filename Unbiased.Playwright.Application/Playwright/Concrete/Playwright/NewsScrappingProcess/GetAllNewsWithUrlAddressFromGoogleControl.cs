using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrapping;
using Unbiased.Playwright.Domain.Enums;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    /// <summary>
    /// This class is responsible for handling the retrieval of news articles with URLs from Google.
    /// It implements the AbstractHandlerChain interface.
    /// </summary>
    public class GetAllNewsWithUrlAddressFromGoogleControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly string _url;
        private readonly LanguageEnums _language;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllNewsWithUrlAddressFromGoogleControl"/> class.
        /// </summary>
        /// <param name="url">The URL to search for news articles.</param>

        public GetAllNewsWithUrlAddressFromGoogleControl(string url, LanguageEnums language, IServiceProvider serviceProvider)
        {
            _url = url;
            _language = language;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Handles the retrieval of news articles with URLs from Google.
        /// </summary>
        /// <returns>A Task that represents the asynchronous retrieval operation.
        /// The Task result contains a list of SaveSearchUrlAndGuidDto objects
        /// representing the retrieved news articles.</returns>

        public async override Task<List<SaveSearchUrlAndGuidDto>> Handle()
        {
            if (!String.IsNullOrEmpty(_url))
            {
                var result = await new GetAllNewsWithUrlAddressFromGoogleMethod(_serviceProvider).GetAllNewsWithUrlAddressFromGoogle(_url, _language);
                return result;
            }

            return await Task.FromResult(Enumerable.Empty<SaveSearchUrlAndGuidDto>().ToList());
        }

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="abstractHandlerChain">The next handler in the chain.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public override Task SetNext(AbstractHandlerChain abstractHandlerChain)
        {
            _abstractHandlerChain = abstractHandlerChain;
            return Task.CompletedTask;
        }
    }
}

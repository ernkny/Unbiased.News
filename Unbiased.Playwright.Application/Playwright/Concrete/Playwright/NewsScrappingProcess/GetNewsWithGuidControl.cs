using Unbiased.Playwright.Application.Dto.PlaywrightDto;
using Unbiased.Playwright.Application.Playwright.Abstract;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess
{
    /// <summary>
    /// This class is responsible for handling the retrieval of news articles with GUIDs.
    /// It implements the AbstractHandlerChain interface.
    /// </summary>
    public class GetNewsWithGuidControl : AbstractHandlerChain
    {
        private AbstractHandlerChain _abstractHandlerChain;
        private readonly List<SaveSearchUrlAndGuidDto> _saveSearchUrlAndGuids;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNewsWithGuidControl"/> class.
        /// </summary>
        /// <param name="saveSearchUrlAndGuids">The list of SaveSearchUrlAndGuidDto objects to retrieve news articles for.</param>
        public GetNewsWithGuidControl(List<SaveSearchUrlAndGuidDto> saveSearchUrlAndGuids, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _saveSearchUrlAndGuids = saveSearchUrlAndGuids;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Handles the retrieval of news articles with GUIDs.
        /// </summary>
        /// <returns>A Task that represents the asynchronous retrieval operation.
        /// The Task result contains a list of News objects representing the retrieved news articles.</returns>
        public async override Task<List<News>> Handle()
        {
            var getNewsContent = new GetNewsWithGuidMethod(_serviceProvider, _eventAndActivityLog);
            var result = await getNewsContent.GetNewsWithGuid(_saveSearchUrlAndGuids);
            return result;
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

using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;

namespace Unbiased.News.Application.Services
{
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;

        public NewsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsQuery(language));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language, string? title)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsWithImageQuery(categoryId, pageNumber, language,title));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId, string? title)
        {
            try
            {

                var result = await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(categoryId,title));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId, string uniqUrlPath,string language)
        {
            try
            {
                var result = await _mediator.Send(new GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery(uniqUrlPath, categoryId, language));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId,string language)
        {
            try
            {

                var result = await _mediator.Send(new GetBannerGeneratedNewsWithImageQuery(categoryId, language));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdAsync(string id)
        {
            try
            {

                var result = await _mediator.Send(new GetGeneratedNewsByIdWithImagePathQuery(id));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlAsync(string UniqUrl)
        {
            try
            {

                var result = await _mediator.Send(new GetGeneratedNewsByUniqUrlWithImageQuery(UniqUrl));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

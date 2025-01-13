using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNews;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;

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
            var result = await _mediator.Send(new GetAllGeneratedNewsQuery(language));
            return result;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber, string language)
        {
            var result = await _mediator.Send(new GetAllGeneratedNewsWithImageQuery( categoryId,pageNumber, language));
            return result;
        }

        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId)
        {
            var result = await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(categoryId));
            return result;
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdAsync(string id)
        {
            var result= await _mediator.Send(new GetGeneratedNewsByIdWithImagePathQuery(id));
            return result;
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlAsync(string UniqUrl)
        {
            var result = await _mediator.Send(new GetGeneratedNewsByUniqUrlWithImageQuery(UniqUrl));
            return result;
        }
    }
}

using MediatR;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;

namespace Unbiased.Dashboard.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly IMediator _mediator;

        public NewsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetGeneratedNewsQuery(requestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                return await _mediator.Send(new GetAllGeneratedNewsWithImageCountQuery(requestDto));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

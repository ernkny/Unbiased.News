using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    public class GetGeneratedNewsByIdWithImageHandler : IRequestHandler<GetGeneratedNewsByIdWithImageQuery, GenerateNewsWithImageDto>
    {
        private readonly INewsRepository _newsRepository;

        public GetGeneratedNewsByIdWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<GenerateNewsWithImageDto> Handle(GetGeneratedNewsByIdWithImageQuery request, CancellationToken cancellationToken)
        {
           return await _newsRepository.GetGeneratedNewsByIdWithImageAsync(request.id);
        }
    }
}

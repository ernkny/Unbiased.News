using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    public class GetNewsWithoutImagesQueryHandler : IRequestHandler<GetNewsWithoutImagesQuery, IEnumerable<GetNewsWithoutImageDto>>
    {
        private readonly INewsImageRepository _newsImageRepository;

        public GetNewsWithoutImagesQueryHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        public async Task<IEnumerable<GetNewsWithoutImageDto>> Handle(GetNewsWithoutImagesQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsImageRepository.GetNewsWithoutImages(request.startDate);
            return result;
        }
    }
}

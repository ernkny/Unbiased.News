using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Application.Cqrs.Queries.Categories;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Services
{
    public sealed class CategoriesService : ICategoriesService
    {
        private readonly IMediator _mediator;

        public CategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var result=await _mediator.Send(new GetCategoriesQuery());
            return result;
        }
    }
}

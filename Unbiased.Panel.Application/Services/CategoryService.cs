using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Categories;

namespace Unbiased.Dashboard.Application.Services
{
    public sealed class CategoryService:ICategoryService
    {
        private readonly IMediator _mediator;
        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllCategoriesQuery());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

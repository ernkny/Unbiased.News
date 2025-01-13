using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetGeneratedNewsByUniqUrlWithImageQuery(string uniqUrl): IRequest<GenerateNewsWithImageDto>;
}

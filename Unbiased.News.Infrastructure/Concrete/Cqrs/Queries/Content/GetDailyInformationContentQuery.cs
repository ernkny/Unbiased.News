using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    public record GetDailyInformationContentQuery:IRequest<Contents>;
}

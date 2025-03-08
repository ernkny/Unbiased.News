using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs
{
    public record GetCustomerByIdQuery(int id):IRequest<Contact>;
}

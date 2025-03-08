using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs
{
    public record DeleteCustomerMessageCommand(int id):IRequest<bool>;
}

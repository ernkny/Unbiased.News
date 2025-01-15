using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record InsertDailyContentCommand(Contents Content) : IRequest<bool>;
}

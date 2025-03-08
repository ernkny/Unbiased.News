using MediatR;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Commands.ContactCommands
{
    public record InsertContactFormCommand(Contact Contact):IRequest<bool>;
}

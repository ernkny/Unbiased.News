using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Application.Validators;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Commands.ContactCommands;

namespace Unbiased.News.Application.Services
{
    public sealed class ContactService : IContactService
    {
        private readonly IMediator _mediator;

        public ContactService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SaveContact(Contact contact)
        {
            try
            {
                var validator = new ContactValidator();
                var validatorResult = validator.Validate(contact);
                if (validatorResult.IsValid) 
                {
                    var result = await _mediator.Send(new InsertContactFormCommand(contact));

                    return result;
                }
                else
                {
                    throw new Exception(validatorResult.Errors.First().ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

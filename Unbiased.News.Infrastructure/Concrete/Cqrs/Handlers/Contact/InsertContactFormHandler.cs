using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Commands.ContactCommands;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Contact
{
    public class InsertContactFormHandler : IRequestHandler<InsertContactFormCommand, bool>
    {
        private readonly IContactRepository _contactRepository;

        public InsertContactFormHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<bool> Handle(InsertContactFormCommand request, CancellationToken cancellationToken)
        {
            return await _contactRepository.SaveContact(request.Contact);
        }
    }
}

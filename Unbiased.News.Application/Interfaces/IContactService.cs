using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    public interface IContactService
    {
        Task<bool> SaveContact(Contact contact);
    }
}

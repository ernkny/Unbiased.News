using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    /// <summary>
    /// Interface for the contact service that handles contact form submissions.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Saves a contact form submission asynchronously.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<bool> SaveContact(Contact contact);
    }
}

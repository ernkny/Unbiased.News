using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IContactRepository
    {
        Task<bool> SaveContact(Contact contact);
    }
}

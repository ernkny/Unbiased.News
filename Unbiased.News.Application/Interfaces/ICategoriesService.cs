using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}

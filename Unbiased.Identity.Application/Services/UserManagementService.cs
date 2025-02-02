using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.Concrete.Cqrs.Queries.UserManagement;

namespace Unbiased.Identity.Application.Services
{
    public class UserManagementService:IUserManagementService
    {
        private readonly IMediator _mediator;

        public UserManagementService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result= await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<int> GetAllUsersCountAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersCountQuery());
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        
    }
}

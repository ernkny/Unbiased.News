using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s.Authentication;
using Unbiased.Identity.Domain.Dto_s.Login;
using Unbiased.Identity.Domain.Dtos.Authentication;
using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
        ClientTokenDto CreateClientToken(Client client);
    }
}

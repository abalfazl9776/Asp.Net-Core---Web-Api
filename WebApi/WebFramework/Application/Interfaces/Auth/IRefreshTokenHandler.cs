using System;
using System.Collections.Generic;
using System.Text;
using WebFramework.Application.Models;

namespace WebFramework.Application.Interfaces.Auth
{
    public interface IRefreshTokenHandler : IRequestHandler<RefreshTokenDto, TokenSelectRequest>
    {
    }
}

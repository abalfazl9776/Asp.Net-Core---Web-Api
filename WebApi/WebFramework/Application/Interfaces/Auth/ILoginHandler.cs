using WebFramework.Application.Models;

namespace WebFramework.Application.Interfaces.Auth
{
    public interface ILoginHandler : IRequestHandler<TokenRequest, TokenSelectRequest>
    {

    }
}

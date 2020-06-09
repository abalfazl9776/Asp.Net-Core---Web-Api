using System.Threading;
using System.Threading.Tasks;

namespace WebFramework.Application.Interfaces
{
    public interface IRequestHandler<TDto, TSelectDto> where TDto : class where TSelectDto : class
    {
        Task<TSelectDto> Handle(TDto input, CancellationToken cancellationToken);
    }
}

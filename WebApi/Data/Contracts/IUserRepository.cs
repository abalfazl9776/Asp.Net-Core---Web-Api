using System.Threading;
using System.Threading.Tasks;
using Entities.User;

namespace Data.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUser(string username);
        Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken);
        Task<User> GetByIdWithTokensAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}
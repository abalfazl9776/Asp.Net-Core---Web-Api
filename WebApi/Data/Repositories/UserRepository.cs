using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository , IScopedDependency
    {
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        [Obsolete]
        public User GetByUser(string username)
        {
            return Table.Single(p => p.UserName == username);
        }

        public Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<User> GetByIdWithTokensAsync(int id, CancellationToken cancellationToken)
        {
            return Table.Where(p => p.Id == id)
                .Include(user => user.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName, cancellationToken);
            if (exists)
                throw new BadRequestException("Username already exists!");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }

        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            //DbContext.Attach(user.RefreshToken);
            //DbContext.Entry<RefreshToken>(user.RefreshToken).State = EntityState.Modified;
            return UpdateAsync(user, cancellationToken);
        }

        //public override void Update(User entity, bool saveNow = true)
        //{
        //    entity.SecurityStamp = Guid.NewGuid();
        //    base.Update(entity, saveNow);
        //}

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }
    }
}

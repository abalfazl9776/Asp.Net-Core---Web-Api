using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebFramework.Application.Interfaces.Auth;

namespace WebFramework.Application.Handlers.Auth
{
    public class LogoutHandler : ILogoutHandler, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<RefreshToken> _repository;

        public LogoutHandler(IUserRepository userRepository, IRepository<RefreshToken> repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }

        public async Task<ApiResultStatusCode> Handle(HttpContext context, CancellationToken cancellationToken)
        {
            var userId = context.User.Identity.GetUserId<int>();
            var user = await _userRepository.Table.Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

            if (user?.RefreshToken == null)
            {
                return ApiResultStatusCode.NotFound;
            }
            await _userRepository.UpdateSecurityStampAsync(user, cancellationToken);
            await _repository.DeleteAsync(user.RefreshToken, cancellationToken);
            return ApiResultStatusCode.Success;
        }
    }
}

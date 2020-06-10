using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using WebFramework.Application.Interfaces.User;
using WebFramework.Application.Models;

namespace WebFramework.Application.Handlers.User
{
    public class GetUsersHandler : IGetUsersHandler, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserSelectDto>> Handle(CancellationToken cancellationToken)
        {
            var usersList = await _userRepository.TableNoTracking.ProjectTo<UserSelectDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return usersList;
        }
    }
}

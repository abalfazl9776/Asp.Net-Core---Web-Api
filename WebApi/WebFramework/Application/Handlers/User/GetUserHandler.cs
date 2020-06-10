using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Data.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebFramework.Application.Interfaces.User;
using WebFramework.Application.Models;
using WebFramework.Application.Models.DTOs;

namespace WebFramework.Application.Handlers.User
{
    public class GetUserHandler : IGetUserHandler, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserSelectDto> Handle(GetByIdRequest id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.TableNoTracking.ProjectTo<UserSelectDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(id.Id), cancellationToken);

            return user;
        }
    }
}
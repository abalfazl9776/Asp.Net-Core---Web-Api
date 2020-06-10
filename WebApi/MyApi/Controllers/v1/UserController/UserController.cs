using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Services.JWT;
using WebFramework.Api;
using WebFramework.Application.Interfaces.User;
using WebFramework.Application.Models;

namespace MyApi.Controllers.v1.UserController
{
    [ApiVersion("1")]
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenFactory _tokenFactory;
        private readonly IGetUsersHandler _getUsersHandler;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager,
            IMapper mapper, ITokenFactory tokenFactory, IGetUsersHandler getUsersHandler)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenFactory = tokenFactory;
            _getUsersHandler = getUsersHandler;
        }

        [HttpGet]
        public async Task<ApiResult<List<UserSelectDto>>> Get(CancellationToken cancellationToken)
        {
            var result = await _getUsersHandler.Handle(cancellationToken);
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<UserSelectDto>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.TableNoTracking.ProjectTo<UserSelectDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<UserSelectDto>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Create user method has been called");

            //var user = new User
            //{
            //    BirthDate = userDto.BirthDate.Date,
            //    FullName = userDto.FullName,
            //    Gender = userDto.Gender,
            //    UserName = userDto.UserName,
            //    Email = userDto.Email
            //};
            //await _userManager.CreateAsync(user, userDto.Password);

            //var result2 = await _roleManager.CreateAsync(new Role
            //{
            //    Name = "Admin",
            //    Description = "writer role"
            //});

            //return Ok(userDto);


            var user = userDto.ToEntity(_mapper);

            await _userManager.CreateAsync(user, userDto.Password);

            //user = await _userManager.FindByNameAsync(user.UserName);

            //var result3 = await _userManager.AddToRoleAsync(user, "Admin");

            var resultDto = await _userRepository.TableNoTracking.ProjectTo<UserSelectDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(user.Id), cancellationToken);

            return resultDto;
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResult<UserSelectDto>> Update(int id, UserDto userDto, CancellationToken cancellationToken)
        {
            var updateUser = await _userRepository.GetByIdAsync(cancellationToken, id);

            updateUser = userDto.ToEntity(_mapper, updateUser);
            updateUser.Id = id;

            await _userRepository.UpdateAsync(updateUser, cancellationToken);

            var resultDto = await _userRepository.TableNoTracking.ProjectTo<UserSelectDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(updateUser.Id), cancellationToken);

            return resultDto;
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            await _userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
    }
}

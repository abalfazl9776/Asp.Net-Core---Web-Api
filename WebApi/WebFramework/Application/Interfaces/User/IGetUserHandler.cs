using System;
using System.Collections.Generic;
using System.Text;
using WebFramework.Application.Models;
using WebFramework.Application.Models.DTOs;

namespace WebFramework.Application.Interfaces.User
{
    public interface IGetUserHandler : IRequestHandler<GetByIdRequest, UserSelectDto>
    {
    }
}

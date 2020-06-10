using System;
using System.Collections.Generic;
using System.Text;
using WebFramework.Application.Models;

namespace WebFramework.Application.Interfaces.User
{
    public interface IGetUsersHandler : IRequestHandler<List<UserSelectDto>>
    {
    }
}

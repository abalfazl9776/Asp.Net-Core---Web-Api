using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using Entities.User;

namespace Services.DataInitializer
{
    public class A_UserAndRolesDataInitializer : IDataInitializer
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public A_UserAndRolesDataInitializer(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void InitializeData()
        {
            InitializeRoles();
            InitializeUsers();
        }

        public void InitializeRoles()
        {
            if (!_roleManager.Roles.AsNoTracking().Any(p => p.Name.Equals(PredefinedRoles.Admin.ToString())))
            {
                var role = new Role
                {
                    Name = PredefinedRoles.Admin.ToString(),
                    Description = "Admin has access to anything."
                };
                var result = _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
        }

        public void InitializeUsers()
        {
            if (!_userManager.Users.AsNoTracking().Any(p => p.UserName == "admin"))
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@site.com",
                    IsActive = true
                    
                };
                var addUser = _userManager.CreateAsync(user, "123123").GetAwaiter().GetResult();
                if (addUser.Succeeded)
                {
                    User admin = _userManager.FindByNameAsync(user.UserName).GetAwaiter().GetResult();
                    var addToRole = _userManager.AddToRoleAsync(admin, PredefinedRoles.Admin.ToString())
                        .GetAwaiter().GetResult();
                }
            }
        }
    }
}
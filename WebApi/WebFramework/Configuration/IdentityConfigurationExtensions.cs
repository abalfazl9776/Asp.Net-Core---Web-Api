﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Common;
using Microsoft.AspNetCore.Identity;
using Data;
using Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebFramework.Configuration
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
                identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
                identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric;
                identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
                identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;


                //// signin service needed for the rest:
                //SingIn Settings
                //identityOptions.SignIn.RequireConfirmedEmail = false;
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

                //Lockout Settings
                identityOptions.Lockout.MaxFailedAccessAttempts = settings.MaxFailedAccessAttempts;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(settings.DefaultLockoutTimeSpanMinutes);
                identityOptions.Lockout.AllowedForNewUsers = settings.AllowedForNewUsers;
            })
            //.AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            //.AddPasswordValidator<CustomPasswordValidator>();
        }

        //public class CustomPasswordValidator : IPasswordValidator<User>
        //{
        //    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        //    {
        //        if (string.IsNullOrEmpty(password))
        //        {
        //            return new Task<IdentityResult>(() => IdentityResult.Success);
        //        }
        //        else
        //        {
        //            PasswordValidator<User> validator = new PasswordValidator<User>();
        //            return validator.ValidateAsync(manager, user, password);
        //        }
        //    }
        //}
    }
}

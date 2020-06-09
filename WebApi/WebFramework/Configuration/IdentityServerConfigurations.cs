using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace WebFramework.Configuration
{
    public static class IdentityServerConfigurations
    {
        public static void AddIs4Authentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44370";
                    options.RequireHttpsMetadata = false; // dev only!
                    options.ApiName = "resourceApi";
                });
        }
    }
}

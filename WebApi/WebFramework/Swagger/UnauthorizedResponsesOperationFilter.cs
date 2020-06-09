using System;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using WebFramework.Filters;

namespace WebFramework.Swagger
{
    public class UnauthorizedResponsesOperationFilter : IOperationFilter
    {
        private readonly bool _includeUnauthorizedAndForbiddenResponses;
        private readonly string _schemeName;

        public UnauthorizedResponsesOperationFilter(bool includeUnauthorizedAndForbiddenResponses, string schemeName = "Bearer")
        {
            this._includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
            this._schemeName = schemeName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        { 
            var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var hasAnonymous = filters.Any(p => p.Filter is AllowAnonymousFilter) || metadata.Any(p => p is AllowAnonymousAttribute);
            if (hasAnonymous) return;

            //var hasAuthorize = context.ControllerActionDescriptor.GetControllerAndActionAttributes(true).OfType<AuthorizeAttribute>().Any();
            var hasAuthorize = filters.Any(p => p.Filter is AuthorizeFilter) || metadata.Any(p => p is AuthorizeAttribute);
            if (!hasAuthorize) return;

            if (_includeUnauthorizedAndForbiddenResponses)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Scheme = _schemeName,
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" }
                    },
                    Array.Empty<string>() //new[] { "readAccess", "writeAccess" }
                }
            });

            //operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            //{
            //    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] {"demo_api"}}}
            //};
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using WebFramework.Api;

namespace WebFramework.Application.Interfaces
{
    public interface IRequestHandler<TDto, TSelectDto> where TDto : class where TSelectDto : class
    {
        Task<TSelectDto> Handle(TDto input, CancellationToken cancellationToken);
    }
    public interface IRequestHandler<TSelectDto> where TSelectDto : class
    {
        Task<TSelectDto> Handle(CancellationToken cancellationToken);
    }

    public interface IRequestHandler
    {
        Task<ApiResultStatusCode> Handle(HttpContext context, CancellationToken cancellationToken);
    }
}

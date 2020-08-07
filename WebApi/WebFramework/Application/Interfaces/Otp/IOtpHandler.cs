using System;
using System.Collections.Generic;
using System.Text;
using WebFramework.Application.Models;

namespace WebFramework.Application.Interfaces.Otp
{
    public interface IOtpHandler : IRequestHandler<OtpValidationRequest, TokenSelectRequest>
    {
    }
}

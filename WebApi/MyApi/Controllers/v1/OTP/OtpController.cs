using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Kavenegar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.SMS.KN;
using Services.SMS.MH;
using Services.SMS.OTP;
using WebFramework.Api;
using WebFramework.Application.Interfaces.Otp;
using WebFramework.Application.Models;
using WebFramework.Application.Models.DTOs;

namespace MyApi.Controllers.v1.OTP
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class OtpController : BaseController
    {
        private readonly IMashhadSmsService _mashhadSmsService;
        private readonly IOtpService _otpService;
        private readonly IKavenegar _kavenegar;
        private readonly IOtpHandler _otpHandler;

        public OtpController(IMashhadSmsService mashhadSmsService, IOtpService otpService, IKavenegar kavenegar, IOtpHandler otpHandler)
        {
            _mashhadSmsService = mashhadSmsService;
            _otpService = otpService;
            _kavenegar = kavenegar;
            _otpHandler = otpHandler;
        }

        [HttpPost("{phone}")]
        //public async Task<ApiResult<string>> TestOtp(CancellationToken cancellationToken)
        public async Task<OtpModel> RequestOtp(string phone, int center, CancellationToken cancellationToken)
        {
            OtpModel model = _otpService.GetCode(phone);
            //if (center == 1)
            //{
            //    var res = await _mashhadSmsService.SendBasicOtp(phone, model.Code);
            //}
            //else
            //{
            //    var res = await _kavenegar.SendOtp(phone, model.Code);
            //}
            return model;
        }
        
        [HttpPost("[action]")]
        public async Task<TokenSelectRequest> ValidateOtp(OtpValidationRequest request, CancellationToken cancellationToken)
        {
            var res = await _otpHandler.Handle(request, cancellationToken);
            return res;
        }
    }
}

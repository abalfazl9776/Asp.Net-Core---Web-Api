using System;
using System.Collections.Generic;
using System.Text;

namespace Services.SMS.OTP
{
    public class OtpModel
    {
        public string Code { get; set; }
        public string Token { get; set; }

        public OtpModel(string code, string token)
        {
            Code = code;
            Token = token;
        }
    }
}

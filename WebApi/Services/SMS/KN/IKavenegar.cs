using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kavenegar.Models;

namespace Services.SMS.KN
{
    public interface IKavenegar
    {
        Task<SendResult> SendOtp(string receptor, string code);
    }
}

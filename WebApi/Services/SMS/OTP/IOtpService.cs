

//using Kavenegar.Models;

namespace Services.SMS.OTP
{
    public interface IOtpService
    {
        OtpModel GetCode(string phone);
        bool ValidateCode(string code, string token, string phone);
    }
}

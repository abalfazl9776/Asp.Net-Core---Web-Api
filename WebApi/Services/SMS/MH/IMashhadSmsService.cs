using System.Threading.Tasks;

//using Kavenegar.Models;

namespace Services.SMS.MH
{
    public interface IMashhadSmsService
    {
        Task<string> SendBasicOtp(string receptor, string code);
        Task<string> SendOtp(string receptor, string code);
    }
}

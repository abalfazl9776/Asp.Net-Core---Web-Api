using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Services.SMS.OTP
{
    public class OtpService : IOtpService, ITransientDependency
    {
        private readonly SiteSettings _siteSetting;
        private readonly IConfiguration _configuration;
        private static string _numbers = "0123456789";

        public OtpService(IOptionsSnapshot<SiteSettings> settings, IConfiguration configuration)
        {
            _siteSetting = settings.Value;
            _configuration = configuration;
        }

        public OtpModel GetCode(string phone)
        {
            var code = GenerateCode();
            var token = GenerateToken(phone, code);
            return new OtpModel(code, token);
        }

        public bool ValidateCode(string code, string token, string phone)
        {
            IConfigurationSection encryptionSection = _configuration.GetSection("Encryption");
            var passPhrase = encryptionSection["StringCipherKey"] + code;

            try
            {
                if (StringCipher.Decrypt(token, passPhrase).Equals(phone))
                {
                    return true;
                }
            }
            catch(Exception exception)
            {
                throw new AppException(ApiResultStatusCode.BadRequest, "کد ارسالی صحیح نیست!", HttpStatusCode.BadRequest);
            }

            return false;
        }

        private string GenerateCode()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder(6);

            for (var i = 0; i < 6; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            return builder.ToString();
        }

        private string GenerateToken(string content, string code)
        {
            IConfigurationSection encryptionSection = _configuration.GetSection("Encryption");
            var passPhrase = encryptionSection["StringCipherKey"] + code;

            return StringCipher.Encrypt(content, passPhrase);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using Kavenegar;
using Kavenegar.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Services.SMS.KN
{
    public class Kavenegar : IKavenegar, ITransientDependency
    {
        private readonly SiteSettings _siteSetting;
        private readonly IConfiguration _configuration;
        
        public Kavenegar(IOptionsSnapshot<SiteSettings> settings, IConfiguration configuration)
        {
            _siteSetting = settings.Value;
            _configuration = configuration;
        }

        // Kavenegar
        public async Task<SendResult> SendOtp(string receptor, string code)
        {
            IConfigurationSection smsSection = _configuration.GetSection("SMS:Kavenegar");

            var api = new KavenegarApi(smsSection["ApiKey"]);
            return api.Send(_siteSetting.KavenegarSmsSettings.Sender, receptor, code);
        }
    }
}

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services.SMS.MH
{
    public class MashhadSmsService : IMashhadSmsService
    {
        private readonly SiteSettings _siteSetting;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MashhadSmsService(IOptionsSnapshot<SiteSettings> settings, IConfiguration configuration, HttpClient httpClient)
        {
            _siteSetting = settings.Value;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> SendBasicOtp(string receptor, string code)
        {
            IConfigurationSection smsSection = _configuration.GetSection("SMS:MashhadHost");

            MhContent content = new MhContent(
                "send", 
                smsSection["Username"], 
                smsSection["Password"], 
                "کد تایید ارمیس: " + code, 
                _siteSetting.MashhadHostSmsSettings.Sender, 
                receptor);

            var request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent(JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                //var model = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                var model = await response.Content.ReadAsStringAsync();
                return model;
            }
            else
            {
                return "Sending OTP encountered an error!";
            }
        }

        public async Task<string> SendOtp(string receptor, string code)
        {
            throw new NotImplementedException("Need to have website url to activating Api Key");
        }

    }
}

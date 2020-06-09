using System.ComponentModel.DataAnnotations;

namespace WebFramework.Application.Models
{
    public class TokenRequest
    {
        //[Required]
        public string grant_type { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string scope { get; set; }

        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
    
    public class TokenSelectRequest
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string access_token { get; set; }
        [Required]
        public string refresh_token { get; set; }
    }
}

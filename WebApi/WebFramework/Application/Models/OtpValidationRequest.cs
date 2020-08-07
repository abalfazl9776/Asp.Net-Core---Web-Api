using System.ComponentModel.DataAnnotations;

namespace WebFramework.Application.Models
{
    public class OtpValidationRequest
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Code { get; set; }
    }
}

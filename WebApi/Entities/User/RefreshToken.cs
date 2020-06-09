using System;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Common;

namespace Entities.User
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public int UserId { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;

        public RefreshToken(string token, DateTime expires, int userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
        }
    }
}

using System;
using System.Security.Cryptography;
using Common;

namespace Services.JWT
{
    public class TokenFactory : ITokenFactory , IScopedDependency
    {
        public string GenerateToken(int size=32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

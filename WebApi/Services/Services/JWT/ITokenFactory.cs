using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Services.Services.JWT
{
    public interface ITokenFactory
    {
        string GenerateToken(int size= 32);
    }
}

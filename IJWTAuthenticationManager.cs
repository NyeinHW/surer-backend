using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surer_backend
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(string username);

    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
    public interface IAccessToken
    {
        DateTime Expiration { get; set; }
        string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

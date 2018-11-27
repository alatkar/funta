using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Jwt.Layer
{
    public class ApiSettingsDto
    {
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string RefreshTokenPath { get; set; }
        public string AccessTokenObjectKey { get; set; }
        public string RefreshTokenObjectKey { get; set; }
        public string AdminRoleName { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace API.Dto
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public IList<string> Roles { get; set; }
    }
}
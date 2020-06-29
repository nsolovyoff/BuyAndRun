using System;
using System.Collections.Generic;

namespace API.Models.User
{
    public class CreateUserModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageBase64 { get; set; }
    }
}

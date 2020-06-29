using System;
using System.Collections.Generic;

namespace API.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

        public int LotsCount { get; set; }
        public int CategoriesCount { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }


        public DateTime RegisteredAt { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using BLL.Resources.Identity.Role;

namespace BLL.Resources.Identity.User
{
    public class UserResource
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }

        public int LotsCount { get; set; }
        public int CategoriesCount { get; set; }

        public DateTime RegisteredAt { get; set; }
        public DateTime? LastActivityAt { get; set; }

        public ICollection<RoleResource> Roles { get; set; }
    }
}

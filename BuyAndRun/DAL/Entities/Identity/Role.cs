using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Identity
{
    public class Role : IdentityRole
    {
        public Role(string roleName) : base(roleName)
        {
        }
        public Role() : base()
        {

        }
        public virtual ICollection<UserToRole> UserRoles { get; set; }
    }
}

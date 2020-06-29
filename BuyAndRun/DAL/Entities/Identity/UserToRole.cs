using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Identity
{
    public class UserToRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}

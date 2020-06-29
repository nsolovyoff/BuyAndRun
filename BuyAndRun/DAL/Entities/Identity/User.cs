using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
            Lots = new Collection<Lot>();
            Categories = new Collection<Category>();
            Auctions = new Collection<AuctionToModerator>();
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RegisteredAt { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<UserToRole> Roles { get; set; }
        public virtual ICollection<AuctionToModerator> Auctions { get; set; }
    }
}

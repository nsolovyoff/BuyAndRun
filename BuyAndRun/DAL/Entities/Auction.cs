using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DAL.Entities
{
    public class Auction
    {
        public Auction()
        {
            Categories = new Collection<Category>();
            Moderators = new Collection<AuctionToModerator>();
        }

        public int AuctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        // created by ?

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<AuctionToModerator> Moderators { get; set; }
    }
}

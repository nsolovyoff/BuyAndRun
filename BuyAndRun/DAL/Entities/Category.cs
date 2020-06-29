using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAL.Entities.Identity;

namespace DAL.Entities
{
    public class Category
    {
        public Category()
        {
            Lots = new Collection<Lot>();
        }

        public int CategoryId { get; set; }
        public string Title { get; set; }

        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }
    }
}

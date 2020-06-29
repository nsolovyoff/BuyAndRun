using System;
using DAL.Entities.Identity;

namespace DAL.Entities
{
    public class Lot
    {
        public int LotId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BuyNowPrice { get; set; }
        public int Bid { get; set; }
        public string BidUser { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Expiring { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string User { get; set; }
    }
}

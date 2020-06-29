using System;
using System.Collections.Generic;

namespace API.Models.Auction
{
    public class AuctionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoriesCount { get; set; }
        public int LotsCount { get; set; }

        public ICollection<string> Moderators { get; set; }
    }
}

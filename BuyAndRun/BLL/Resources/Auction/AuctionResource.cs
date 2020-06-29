using System;
using System.Collections.Generic;
using BLL.Resources.Identity.User;
using BLL.Resources.Lot;

namespace BLL.Resources.Auction
{
    public class AuctionResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoriesCount { get; set; }
        public int LotsCount { get; set; }

        public ICollection<UserResource> Moderators { get; set; }
    }
}

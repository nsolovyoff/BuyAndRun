using DAL.Entities.Identity;

namespace DAL.Entities
{
    public class AuctionToModerator
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }
    }
}

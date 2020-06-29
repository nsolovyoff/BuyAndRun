using System;

namespace BLL.Resources.Lot
{
    public class UpdateLotResource
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int BuyNowPrice { get; set; }
        public int Bid { get; set; }
        public string BidUser { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Expiring { get; set; }
        public int CategoryId { get; set; }
    }
}

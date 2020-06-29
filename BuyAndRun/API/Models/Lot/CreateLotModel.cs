using System;

namespace API.Models.Lot
{
    public class CreateLotModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int BuyNowPrice { get; set; }
        public int Bid { get; set; }
        public string BidUser { get; set; }
        public DateTime Expiring { get; set; }
        public int CategoryId { get; set; }
        public string ImageBase64 { get; set; }
        public string User { get; set; }
    }
}

namespace BLL.Resources.Auction
{
    public class CreateAuctionResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

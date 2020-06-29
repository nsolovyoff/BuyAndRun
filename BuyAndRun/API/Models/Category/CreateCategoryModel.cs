namespace API.Models.Category
{
    public class CreateCategoryModel
    {
        public string Title { get; set; }
        public int ParentAuctionId { get; set; }
        public string StartedBy { get; set; }
    }
}

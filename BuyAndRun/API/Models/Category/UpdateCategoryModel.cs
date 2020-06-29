namespace API.Models.Category
{
    public class UpdateCategoryModel
    {
        public string Title { get; set; }
        public int ParentAuctionId { get; set; }
    }
}

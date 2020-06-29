using System;

namespace API.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int LotsCount { get; set; }

        public int ParentAuctionId { get; set; }
        public string StartedBy { get; set; }
    }
}

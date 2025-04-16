namespace AgrifoodManagement.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<CategoryViewModel> Children { get; set; } = new List<CategoryViewModel>();
    }
}

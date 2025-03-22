namespace AgrifoodManagement.Web.Models
{
    public class SidebarViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool HasChild { get; set; }
        public bool Expanded { get; set; }
        public bool IsPro { get; set; }
        public string Url { get; set; }
        public string IconCss { get; set; }
    }
}

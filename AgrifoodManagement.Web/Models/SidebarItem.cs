namespace AgrifoodManagement.Web.Models
{
    public class SidebarItem
    {
        public string Id { get; set; }
        public string Pid { get; set; }
        public string Name { get; set; }
        public bool HasChild { get; set; }
        public bool Expanded { get; set; }
        public string Badge { get; set; }
    }
}

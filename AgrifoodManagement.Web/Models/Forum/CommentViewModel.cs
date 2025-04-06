namespace AgrifoodManagement.Web.Models.Forum
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public UserViewModel Author { get; set; }
        public string TimeAgo { get; set; }
    }
}

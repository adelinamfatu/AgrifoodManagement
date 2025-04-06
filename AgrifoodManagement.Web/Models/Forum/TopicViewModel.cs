namespace AgrifoodManagement.Web.Models.Forum
{
    public class TopicViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public UserViewModel Author { get; set; }
        public string LatestReplyAuthor { get; set; }
        public string LatestReplyTimeAgo { get; set; }
        public int CommentsCount { get; set; }
        public List<UserViewModel> TopCommenters { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}

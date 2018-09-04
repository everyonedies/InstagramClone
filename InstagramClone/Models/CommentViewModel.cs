using System;

namespace InstagramClone.Models
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public AppUserViewModel User { get; set; }
        public PostViewModel Post { get; set; }
    }
}

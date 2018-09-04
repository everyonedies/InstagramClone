using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public class PostViewModel
    {
        public string Text { get; set; }
        public string Picture { get; set; }
        public DateTime Date { get; set; }
        public ICollection<LikeViewModel> Likes { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace InstagramClone.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PicturePreview { get; set; }
        public string PictureView { get; set; }
        public DateTime Date { get; set; }
        public AppUserViewModel User { get; set; }
        public ICollection<LikeViewModel> Likes { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
    }
}

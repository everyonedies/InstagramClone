using System;
using System.Collections.Generic;

namespace InstagramClone.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public string PictureView { get; set; }
        public string PicturePreview { get; set; }
        public DateTime Date { get; set; }

        public virtual AppUser User { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<TagPost> TagPosts { get; set; }
    }
}

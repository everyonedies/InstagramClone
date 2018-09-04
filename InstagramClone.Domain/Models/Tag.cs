using System.Collections.Generic;

namespace InstagramClone.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<TagPost> TagPosts { get; set; }
    }
}

using System;

namespace InstagramClone.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}

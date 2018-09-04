using System;

namespace InstagramClone.Models
{
    public class LikeViewModel
    {
        public AppUserViewModel User { get; set; }
        public PostViewModel Post { get; set; }
        public DateTime Date { get; set; }
    }
}

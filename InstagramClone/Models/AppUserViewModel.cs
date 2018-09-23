using System.Collections.Generic;

namespace InstagramClone.Models
{
    public class AppUserViewModel
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }
        public string WebSite { get; set; }
        public int? NumberOfFollowers { get; set; }
        public int? NumberOfFollowing { get; set; }
        public ICollection<PostViewModel> Posts { get; set; }
    }
}

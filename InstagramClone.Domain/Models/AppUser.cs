using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InstagramClone.Domain.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Alias")]
        [Required(ErrorMessage = "The alias is required")]
        [RegularExpression(@"^[a-z0-9_]+$", ErrorMessage = "Alias must contains only numbers, underscore and latin letters in lower case")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Alias must be at least 1 characters")]
        public string Alias { get; set; }

        [Display(Name = "Web-site address")]
        [Url]
        public string WebSite { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Real name")]
        public string RealName { get; set; }

        public string Picture { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Message> OutgoingMessages { get; set; }

        public virtual ICollection<Message> IncomingMessages { get; set; }

        public virtual ICollection<Follower> Followers { get; set; }

        public virtual ICollection<Follower> Following { get; set; }
    }
}

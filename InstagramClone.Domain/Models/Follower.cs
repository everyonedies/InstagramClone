namespace InstagramClone.Domain.Models
{
    public class Follower
    {
        public int Id { get; set; }

        public string WhoFollowsId { get; set; }
        public string ForWhomFollowsId { get; set; }

        public virtual AppUser WhoFollows { get; set; }
        public virtual AppUser ForWhomFollows { get; set; }
    }
}

namespace InstagramClone.Domain.Models
{
    public class TagPost
    {
        public int TagId { get; set; }
        public int PostId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Post Post { get; set; }
    }
}

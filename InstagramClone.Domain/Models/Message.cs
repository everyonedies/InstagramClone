using System;

namespace InstagramClone.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string SenderId { get; set; }
        public string RecipientId { get; set; }

        public virtual AppUser Sender { get; set; }
        public virtual AppUser Recipient { get; set; }
    }
}

using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InstagramClone.Domain.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagPost> TagPosts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Alias)
            .IsUnique();

            modelBuilder.Entity<TagPost>()
            .HasKey(tp => new { tp.TagId, tp.PostId });

            modelBuilder.Entity<TagPost>()
                .HasOne(tp => tp.Tag)
                .WithMany(b => b.TagPosts)
                .HasForeignKey(tp => tp.TagId);

            modelBuilder.Entity<TagPost>()
                .HasOne(tp => tp.Post)
                .WithMany(c => c.TagPosts)
                .HasForeignKey(tp => tp.PostId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.OutgoingMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.IncomingMessages)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasOne(m => m.WhoFollows)
                .WithMany(u => u.Following)
                .HasForeignKey(m => m.WhoFollowsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasOne(m => m.ForWhomFollows)
                .WithMany(u => u.Followers)
                .HasForeignKey(m => m.ForWhomFollowsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

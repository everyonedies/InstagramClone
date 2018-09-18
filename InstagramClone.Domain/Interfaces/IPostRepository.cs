using InstagramClone.Domain.Models;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostRepository : IRepository<Post>, IAsyncRepository<Post>
    {
        Post GetByIdWithItems(int Id);
        ICollection<Post> GetPostsByTag(Tag tag);
        ICollection<Post> GetPostsWithItemsByTag(Tag tag);
    }
}

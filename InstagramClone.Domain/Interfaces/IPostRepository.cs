using InstagramClone.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostRepository : IRepository<Post>, IAsyncRepository<Post>
    {
        Task<Post> GetByIdWithItems(int Id);
        Task<ICollection<Post>> GetPostsByTag(Tag tag);
        Task<ICollection<Post>> GetPostsWithItemsByTag(Tag tag);
    }
}

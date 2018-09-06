using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostRepository : IRepository<Post>, IAsyncRepository<Post>
    {
        Post GetByIdWithItems(int Id);
    }
}

using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface ILikeRepository : IRepository<Like>, IAsyncRepository<Like>
    {
    }
}

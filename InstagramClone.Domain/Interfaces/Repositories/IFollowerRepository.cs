using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface IFollowerRepository : IRepository<Follower>, IAsyncRepository<Follower>
    {
    }
}

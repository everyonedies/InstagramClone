using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IFollowerRepository Followers { get; }
        IPostRepository Posts { get; }
        Task SaveAsync();
    }
}

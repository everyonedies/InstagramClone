using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IUserRepository Users { get; }
        IFollowerRepository Followers { get; }
        Task SaveAsync();
    }
}

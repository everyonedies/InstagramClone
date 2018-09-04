using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserRepository : IRepository<AppUser>, IAsyncRepository<AppUser>
    {
        AppUser GetByAliasWithItems(string alias);
    }
}

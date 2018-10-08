using InstagramClone.Domain.Models;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserRepository : IRepository<AppUser>, IAsyncRepository<AppUser>
    {
        Task<AppUser> GetByAliasWithItems(string alias);
    }
}

using InstagramClone.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface ITagRepository : IRepository<Tag>, IAsyncRepository<Tag>
    {
        Task<ICollection<Tag>> GetTagsByNameWithItems(string text);
    }
}

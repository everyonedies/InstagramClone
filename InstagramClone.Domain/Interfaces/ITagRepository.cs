using InstagramClone.Domain.Models;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface ITagRepository : IRepository<Tag>, IAsyncRepository<Tag>
    {
        ICollection<Tag> GetTagsByNameWithItems(string text);
    }
}

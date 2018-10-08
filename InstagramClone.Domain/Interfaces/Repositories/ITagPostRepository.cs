using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface ITagPostRepository : IRepository<TagPost>, IAsyncRepository<TagPost>
    {
    }
}

using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface ITagRepository : IRepository<Tag>, IAsyncRepository<Tag>
    {
    }
}

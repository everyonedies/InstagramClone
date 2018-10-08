using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Infrastucture
{
    public class TagPostRepository : EfRepository<TagPost>, ITagPostRepository
    {
        public TagPostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

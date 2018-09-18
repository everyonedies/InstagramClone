using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Infrastucture
{
    public class TagRepository : EfRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

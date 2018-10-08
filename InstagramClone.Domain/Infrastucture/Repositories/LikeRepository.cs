using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Infrastucture
{
    public class LikeRepository : EfRepository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

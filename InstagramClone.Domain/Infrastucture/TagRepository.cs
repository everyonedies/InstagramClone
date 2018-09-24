using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Infrastucture
{
    public class TagRepository : EfRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<ICollection<Tag>> GetTagsByNameWithItems(string text)
        {
            return Task.Run(() => {
                List<Tag> tags = _dbContext.Tags.Include(t => t.TagPosts).ToList();
                ICollection<Tag> res = tags.Where(t => t.Text.Contains(text) && t.TagPosts.Count() > 0).ToList();
                return res;
            });
        }
    }
}

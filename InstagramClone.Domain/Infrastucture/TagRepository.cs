using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InstagramClone.Domain.Infrastucture
{
    public class TagRepository : EfRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Tag> GetTagsByNameWithItems(string text)
        {
            var tags = _dbContext.Tags.Include(t => t.TagPosts).ToList();
            var res = tags.Where(t => t.Text.Contains(text) && t.TagPosts.Count() > 0).ToList();
            return res;
        }
    }
}

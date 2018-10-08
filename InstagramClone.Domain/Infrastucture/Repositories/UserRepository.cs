using System.Linq;
using System.Threading.Tasks;
using InstagramClone.Domain.Infrastructure;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Infrastucture
{
    public class UserRepository : EfRepository<AppUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<AppUser> GetByAliasWithItems(string alias)
        {
            return Task.Run(() => {
                var user = _dbContext.Users.Where(u => u.Alias == alias).FirstOrDefault();

                if (user != null)
                {
                    _dbContext.Entry(user).Collection(u => u.Followers).Load();
                    _dbContext.Entry(user).Collection(u => u.Following).Load();
                    _dbContext.Entry(user).Collection(u => u.Posts).Load();
                    _dbContext.Entry(user).Collection(u => u.Likes).Load();

                    foreach (var i in user.Followers)
                    {
                        _dbContext.Entry(i).Reference(u => u.ForWhomFollows).Load();
                        _dbContext.Entry(i).Reference(u => u.WhoFollows).Load();
                    }

                    foreach (var i in user.Following)
                    {
                        _dbContext.Entry(i).Reference(u => u.ForWhomFollows).Load();
                        _dbContext.Entry(i).Reference(u => u.WhoFollows).Load();
                    }

                    foreach (var i in user.Posts)
                    {
                        _dbContext.Entry(i).Reference(p => p.User).Load();
                        _dbContext.Entry(i).Collection(p => p.Likes).Load();
                        _dbContext.Entry(i).Collection(p => p.Comments).Load();
                    }

                    foreach (var i in user.Likes)
                    {
                        _dbContext.Entry(i).Reference(l => l.Post).Load();
                    }
                }
                return user;
            });
        }
    }
}

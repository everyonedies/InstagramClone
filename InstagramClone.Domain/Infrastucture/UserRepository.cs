﻿using System.Linq;
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

        public AppUser GetByAliasWithItems(string alias)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Alias == alias);

            if (user != null)
            {
                _dbContext.Entry(user).Collection(u => u.Followers).Load();
                _dbContext.Entry(user).Collection(u => u.Following).Load();
                _dbContext.Entry(user).Collection(u => u.Posts).Load();

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
            }

            return user;
        }
    }
}
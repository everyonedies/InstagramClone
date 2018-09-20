using System.Collections.Generic;
using System.Linq;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ICollection<Post> GetUserNews(AppUser user)
        {
            AppUser appUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);
            IEnumerable<Post> posts = new List<Post>();

            foreach (Follower i in appUser.Following)
            {
                AppUser following = unitOfWork.Users.GetByAliasWithItems(i.ForWhomFollows.Alias);
                posts = posts.Concat(following.Posts);
            }

            posts = posts.OrderByDescending(p => p.Date);
            return posts.ToList();
        }

        public ICollection<AppUser> FindUsersByAlias(string alias)
        {
            var users = unitOfWork.Users.List(u => u.Alias.Contains(alias)).ToList();

            return users;
        }

        public ICollection<AppUser> GetUserFollowers(AppUser user)
        {
            AppUser appUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);
            ICollection<AppUser> userFollowers = appUser.Followers.Select(u => u.WhoFollows).ToList();

            return userFollowers;
        }

        public ICollection<AppUser> GetUserFollowing(AppUser user)
        {
            AppUser appUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);
            ICollection<AppUser> userFollowing = appUser.Following.Select(u => u.ForWhomFollows).ToList();

            return userFollowing;
        }

        public bool IsUserFollowing(AppUser currentUser, AppUser targetUser)
        {
            bool result = false;

            var following = GetFollowing(currentUser, targetUser);

            if (following != null)
            {
                result = true;
            }

            return result;
        }

        public bool Follow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias && !IsUserFollowing(currentUser, targetUser))
            {
                Follower follower = new Follower { ForWhomFollows = targetUser, WhoFollows = currentUser };
                currentUser.Following.Add(follower);
                unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Unfollow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias && IsUserFollowing(currentUser, targetUser))
            {
                var following = GetFollowing(currentUser, targetUser);
                unitOfWork.Followers.Delete(following);
                unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        private Follower GetFollowing(AppUser currentUser, AppUser targetUser)
        {
            var following = currentUser.Following.FirstOrDefault(u => u.ForWhomFollows.Alias == targetUser.Alias);
            return following;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<ICollection<AppUser>> GetTopUsers(int count)
        {
            return Task.Run(() => {
                ICollection<AppUser> topUsers = unitOfWork.Users.ListAll()
                .Select(u => unitOfWork.Users.GetByAliasWithItems(u.Alias).Result)
                .OrderByDescending(u => u.Followers.Count())
                .Take(count)
                .ToList();
                return topUsers;
            });
        }

        public Task<ICollection<Post>> GetUserNews(string alias)
        {
            return Task.Run(async () =>
            {
                AppUser appUser = await unitOfWork.Users.GetByAliasWithItems(alias);
                IEnumerable<Post> posts = new List<Post>();

                foreach (Follower i in appUser.Following)
                {
                    AppUser following = await unitOfWork.Users.GetByAliasWithItems(i.ForWhomFollows.Alias);
                    posts = posts.Concat(following.Posts);
                }

                posts = posts.OrderByDescending(p => p.Date);
                ICollection<Post> result = posts.ToList();

                return result;
            });
        }

        public Task<ICollection<Post>> GetUserLikedPosts(string alias)
        {
            return Task.Run(async () => {
                AppUser appUser = await unitOfWork.Users.GetByAliasWithItems(alias);

                IEnumerable<Post> liked = appUser.Likes.Where(l => l.Post != null).Select(l => l.Post);
                ICollection<Post> loadedPosts = liked.Select(p => unitOfWork.Posts.GetByIdWithItems(p.Id).Result).OrderByDescending(p => p.Date).ToList();

                return loadedPosts;
            });
        }

        public async Task<ICollection<AppUser>> FindUsersByAlias(string alias)
        {
            var users = await unitOfWork.Users.ListAsync(u => u.Alias.Contains(alias));
            return users;
        }

        public async Task<ICollection<AppUser>> GetUserFollowers(string alias)
        {
            AppUser appUser = await unitOfWork.Users.GetByAliasWithItems(alias);
            ICollection<AppUser> userFollowers = appUser.Followers.Select(u => u.WhoFollows).ToList();

            return userFollowers;
        }

        public async Task<ICollection<AppUser>> GetUserFollowing(string alias)
        {
            AppUser appUser = await unitOfWork.Users.GetByAliasWithItems(alias);
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

        public async Task<bool> Follow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias && !IsUserFollowing(currentUser, targetUser))
            {
                Follower follower = new Follower { ForWhomFollows = targetUser, WhoFollows = currentUser };
                currentUser.Following.Add(follower);
                await unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Unfollow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias && IsUserFollowing(currentUser, targetUser))
            {
                var following = GetFollowing(currentUser, targetUser);
                unitOfWork.Followers.Delete(following);
                await unitOfWork.SaveAsync();
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

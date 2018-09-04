using System;
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

        public IEnumerable<string> FindUsersByAlias(string alias)
        {
            var users = unitOfWork.Users.List(u => u.Alias.Contains(alias)).Select(u => u.Alias).OrderBy(u => u).AsEnumerable();

            return users;
        }

        public IEnumerable<string> GetUserFollowers(string alias)
        {
            var appUser = unitOfWork.Users.GetByAliasWithItems(alias);
            var userFollowers = appUser.Followers.Select(u => u.WhoFollows.Alias).OrderBy(u => u).ToList();

            return userFollowers;
        }

        public IEnumerable<string> GetUserFollowing(string alias)
        {
            var appUser = unitOfWork.Users.GetByAliasWithItems(alias);
            var userFollowing = appUser.Following.Select(u => u.ForWhomFollows.Alias).OrderBy(u => u).ToList();

            return userFollowing;
        }

        public bool IsUserFollowing(AppUser currentUser, AppUser targetUser)
        {
            bool result = false;

            var isCurrentUserFollowing = GetFollowing(currentUser, targetUser) == null;

            if (!isCurrentUserFollowing)
            {
                result = true;
            }

            return result;
        }

        public void Follow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias)
            {
                Follower follower = new Follower { ForWhomFollows = targetUser, WhoFollows = currentUser };
                currentUser.Following.Add(follower);
                unitOfWork.SaveAsync();
            }
        }

        public void Unfollow(AppUser currentUser, AppUser targetUser)
        {
            if (currentUser.Alias != targetUser.Alias)
            {
                var following = GetFollowing(currentUser, targetUser);
                unitOfWork.Followers.Delete(following);
                unitOfWork.SaveAsync();
            }
        }

        private Follower GetFollowing(AppUser currentUser, AppUser targetUser)
        {
            var following = currentUser.Following.FirstOrDefault(u => u.ForWhomFollows.Alias == targetUser.Alias);
            return following;
        }
    }
}

using InstagramClone.Domain.Models;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserService
    {
        ICollection<AppUser> FindUsersByAlias(string alias);
        ICollection<AppUser> GetUserFollowers(string alias);
        ICollection<AppUser> GetUserFollowing(string alias);
        ICollection<Post> GetUserNews(string alias);
        bool IsUserFollowing(AppUser currentUser, AppUser targetUser);
        bool Follow(AppUser currentUser, AppUser targetUser);
        bool Unfollow(AppUser currentUser, AppUser targetUser);
    }
}

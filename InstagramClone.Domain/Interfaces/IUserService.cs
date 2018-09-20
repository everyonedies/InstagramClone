using InstagramClone.Domain.Models;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserService
    {
        ICollection<AppUser> FindUsersByAlias(string alias);
        ICollection<AppUser> GetUserFollowers(AppUser user);
        ICollection<AppUser> GetUserFollowing(AppUser user);
        ICollection<Post> GetUserNews(AppUser user);
        bool IsUserFollowing(AppUser currentUser, AppUser targetUser);
        bool Follow(AppUser currentUser, AppUser targetUser);
        bool Unfollow(AppUser currentUser, AppUser targetUser);
    }
}

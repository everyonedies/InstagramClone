using InstagramClone.Domain.Models;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserService
    {
        IEnumerable<string> FindUsersByAlias(string alias);
        IEnumerable<string> GetUserFollowers(string alias);
        IEnumerable<string> GetUserFollowing(string alias);
        bool IsUserFollowing(AppUser currentUser, AppUser targetUser);
        void Follow(AppUser currentUser, AppUser targetUser);
        void Unfollow(AppUser currentUser, AppUser targetUser);
    }
}

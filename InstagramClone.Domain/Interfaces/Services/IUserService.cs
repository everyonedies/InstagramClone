using InstagramClone.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<AppUser>> GetTopUsers(int count);
        Task<ICollection<AppUser>> FindUsersByAlias(string alias);
        Task<ICollection<AppUser>> GetUserFollowers(string alias);
        Task<ICollection<AppUser>> GetUserFollowing(string alias);
        Task<ICollection<Post>> GetUserNews(string alias);
        Task<ICollection<Post>> GetUserLikedPosts(string alias);
        bool IsUserFollowing(AppUser currentUser, AppUser targetUser);
        Task<bool> Follow(AppUser currentUser, AppUser targetUser);
        Task<bool> Unfollow(AppUser currentUser, AppUser targetUser);
    }
}

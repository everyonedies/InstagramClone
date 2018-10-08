using InstagramClone.Domain.Models;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostService
    {
        Task AddPostCaption(int postId, string text, string userAlias);
        Task AddNewComment(int postId, string text, string userAlias);
        Task AddPostTags(int postId, string tags, string userAlias);
        Task RemovePostTags(int postId, string userAlias);
        bool IsLiked(Post post, AppUser user);
        Task<int> Like(Post post, AppUser user);
        Task<int> Unlike(Post post, AppUser user);
    }
}

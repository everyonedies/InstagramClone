using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostService
    {
        bool AddPostCaption(int postId, string text, string userAlias);
        bool AddNewComment(int postId, string text, string userAlias);
        bool IsLiked(Post post, AppUser user);
        bool Like(Post post, AppUser user);
        bool Unlike(Post post, AppUser user);
    }
}

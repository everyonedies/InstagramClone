using InstagramClone.Domain.Models;

namespace InstagramClone.Domain.Interfaces
{
    public interface IPostService
    {
        void AddPostCaption(int postId, string text, string userAlias);
        void AddNewComment(int postId, string text, string userAlias);
        void AddPostTags(int postId, string tags, string userAlias);
        void RemovePostTags(int postId, string userAlias);
        bool IsLiked(Post post, AppUser user);
        int Like(Post post, AppUser user);
        int Unlike(Post post, AppUser user);
    }
}

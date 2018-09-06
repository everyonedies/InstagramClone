namespace InstagramClone.Domain.Interfaces
{
    public interface IPostService
    {
        bool AddPostCaption(int postId, string text, string userAlias);
        bool AddNewComment(int postId, string text, string userAlias);
    }
}

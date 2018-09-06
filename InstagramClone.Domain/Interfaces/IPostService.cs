namespace InstagramClone.Domain.Interfaces
{
    public interface IPostService
    {
        bool AddNewComment(int postId, string text, string userAlias);
    }
}

namespace InstagramClone.Domain.Interfaces
{
    public interface IAdminService
    {
        void DeletePost(int postId);
        void DeleteComment(int commentId);
        void BlockUser(string alias);
        void UnblockUser(string alias);
        void SetModerRoleForUser(string alias);
        void UnsetModerRoleForUser(string alias);
    }
}

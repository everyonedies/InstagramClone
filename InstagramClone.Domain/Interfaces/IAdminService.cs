using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IAdminService
    {
        void DeletePost(int postId);
        void DeleteComment(int commentId);
        void BanUser(string alias);
        void UnbanUser(string alias);
        void SetModerRole(string alias);
        void UnsetModerRole(string alias);
    }
}

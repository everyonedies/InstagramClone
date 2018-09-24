using InstagramClone.Domain.Models;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IAdminService
    {
        Task DeletePost(AppUser currentUser, int postId);
        void DeleteComment(int commentId);
        void BlockUser(string alias);
        void UnblockUser(string alias);
        void SetModerRoleForUser(string alias);
        void UnsetModerRoleForUser(string alias);
    }
}

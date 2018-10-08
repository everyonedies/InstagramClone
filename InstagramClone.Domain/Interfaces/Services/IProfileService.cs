using InstagramClone.Domain.Models;
using System.Drawing;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Interfaces
{
    public interface IProfileService
    {
        Task SetProfilePhoto(AppUser user, Image image, string imageExt);
        Task AddNewPost(AppUser user, Image image, string imageExt);
        Task<bool> DeletePost(AppUser user, int postId);
    }
}

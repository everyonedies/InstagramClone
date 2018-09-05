using InstagramClone.Domain.Models;
using System.Drawing;

namespace InstagramClone.Domain.Interfaces
{
    public interface IProfileService
    {
        void SetProfilePhoto(AppUser user, Image image, string imageExt);
        void AddNewPost(AppUser user, Image image, string imageExt);
    }
}

using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public void BlockUser(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteComment(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeletePost(AppUser currentUser, int postId)
        {
            AppUser appUser = await unitOfWork.Users.GetByAliasWithItems(currentUser.Alias);

            Post appUserPost = appUser.Posts.FirstOrDefault(p => p.Id == postId);
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);

            if (post != null)
            {
                AppUser postOwner = post.User;

                var postOwnerRoles = await userManager.GetRolesAsync(postOwner);
                string admRolePostOwner = postOwnerRoles.FirstOrDefault(r => r == "admin");
                string moderRolePostOwner = postOwnerRoles.FirstOrDefault(r => r == "moder");

                bool permission = false;
                bool isAdmin = await userManager.IsInRoleAsync(currentUser, "admin");
                bool isModer = await userManager.IsInRoleAsync(currentUser, "moder");

                if (isAdmin && appUserPost != null || (admRolePostOwner == null && postOwner.Alias != appUser.Alias))
                    permission = true;

                if (isModer && appUserPost != null || (moderRolePostOwner == null && admRolePostOwner == null && postOwner.Alias != appUser.Alias))
                    permission = true;

                if (permission)
                    unitOfWork.Posts.Delete(post);
                else
                    throw new ArgumentException("The user doesn't have a permission for this action");
            }
            else throw new ArgumentException("Invalid post ID");
        }

        public void SetModerRoleForUser(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void UnblockUser(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void UnsetModerRoleForUser(string alias)
        {
            throw new System.NotImplementedException();
        }
    }
}

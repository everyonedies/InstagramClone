using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using System;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void BanUser(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteComment(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePost(int postId)
        {
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            if (post != null)
            {
                unitOfWork.Posts.Delete(post);
            }
            else throw new ArgumentException("Invalid post ID");
        }

        public void SetModerRole(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void UnbanUser(string alias)
        {
            throw new System.NotImplementedException();
        }

        public void UnsetModerRole(string alias)
        {
            throw new System.NotImplementedException();
        }
    }
}

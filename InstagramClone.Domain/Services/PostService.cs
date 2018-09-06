using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using System;
using System.Linq;

namespace InstagramClone.Domain.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool AddNewComment(int postId, string text, string userAlias)
        {
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = unitOfWork.Users.GetByAliasWithItems(userAlias);

            if (post != null && user != null && text != null && text != string.Empty)
            {
                Comment comment = new Comment
                {
                    Date = DateTime.Now,
                    Text = text,
                    User = user
                };

                post.Comments.Add(comment);
                unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddPostCaption(int postId, string text, string userAlias)
        {
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = unitOfWork.Users.GetByAliasWithItems(userAlias);

            var check = user.Posts.FirstOrDefault(p => p.Id == postId);

            if (check != null && post != null && user != null && text != null && text != string.Empty)
            {
                post.Text = text;
                unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

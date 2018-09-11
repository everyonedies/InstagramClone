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
            else return false;
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
            else return false;
        }

        public bool Like(Post post, AppUser user)
        {
            if (post != null && user != null && !IsLiked(post, user))
            {
                Like like = new Like
                {
                    User = user,
                    Date = DateTime.Now
                };
                post.Likes.Add(like);
                unitOfWork.SaveAsync();
                return true;
            }
            else return false;
        }

        public bool Unlike(Post post, AppUser user)
        {
            if (post != null && user != null && IsLiked(post, user))
            {
                var like = GetLike(post, user);
                unitOfWork.Likes.Delete(like);
                unitOfWork.SaveAsync();
                return true;
            }
            else return false;
        }

        public bool IsLiked(Post post, AppUser user)
        {
            var checkLike = GetLike(post, user);

            if (checkLike == null)
                return false;
            else
                return true;
        }

        private Like GetLike(Post post, AppUser user)
        {
            var like = post.Likes.FirstOrDefault(l => l.User.Alias == user.Alias);
            return like;
        }
    }
}

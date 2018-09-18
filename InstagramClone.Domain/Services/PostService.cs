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

            if (check != null && post != null && user != null)
            {
                post.Text = text;
                unitOfWork.SaveAsync();
                return true;
            }
            else return false;
        }

        public bool AddPostTags(int postId, string tags, string userAlias)
        {
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = unitOfWork.Users.GetByAliasWithItems(userAlias);

            var check = user.Posts.FirstOrDefault(p => p.Id == postId);

            if (user != null && post != null && check != null && tags != null)
            {
                var res = tags.Split("#").Where(s => s != "").Select(s => s.Trim());
                foreach (var i in res)
                {
                    Tag tag = unitOfWork.Tags.List(t => t.Text == i).FirstOrDefault();
                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Text = i
                        };
                        unitOfWork.Tags.Add(tag);
                    }

                    TagPost tagPost = post.TagPosts.Where(tp => tp.Tag == tag).FirstOrDefault();
                    if (tagPost == null)
                    {
                        tagPost = new TagPost
                        {
                            Tag = tag,
                            Post = post
                        };
                        unitOfWork.TagPost.Add(tagPost);
                    }
                }
                unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public bool RemovePostTags(int postId, string userAlias)
        {
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = unitOfWork.Users.GetByAliasWithItems(userAlias);

            var check = user.Posts.FirstOrDefault(p => p.Id == postId);

            if (user != null && post != null && check != null)
            {
                var tp = post.TagPosts.ToList();
                foreach (var i in tp)
                {
                    unitOfWork.TagPost.Delete(i);
                }
                unitOfWork.SaveAsync();
                return true;
            }
            return false;
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

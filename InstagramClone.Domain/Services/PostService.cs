using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Domain.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddNewComment(int postId, string text, string userAlias)
        {
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = await unitOfWork.Users.GetByAliasWithItems(userAlias);

            if (post != null && user != null && text != null && text != string.Empty)
            {
                Comment comment = new Comment
                {
                    Date = DateTime.Now,
                    Text = text,
                    User = user
                };

                post.Comments.Add(comment);
                await unitOfWork.SaveAsync();
            }
            else throw new ArgumentException();
        }

        public async Task AddPostCaption(int postId, string text, string userAlias)
        {
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = await unitOfWork.Users.GetByAliasWithItems(userAlias);

            var check = user.Posts.FirstOrDefault(p => p.Id == postId);

            if (check != null && post != null && user != null)
            {
                post.Text = text;
                await unitOfWork.SaveAsync();
            }
            else throw new ArgumentException();
        }

        public async Task AddPostTags(int postId, string tags, string userAlias)
        {
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = await unitOfWork.Users.GetByAliasWithItems(userAlias);

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
                await unitOfWork.SaveAsync();
            }
            else throw new ArgumentException();
        }

        public async Task RemovePostTags(int postId, string userAlias)
        {
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser user = await unitOfWork.Users.GetByAliasWithItems(userAlias);

            var check = user.Posts.FirstOrDefault(p => p.Id == postId);

            if (user != null && post != null && check != null)
            {
                var tp = post.TagPosts.ToList();
                foreach (var i in tp)
                {
                    unitOfWork.TagPost.Delete(i);
                }
                await unitOfWork.SaveAsync();
            }
            else throw new ArgumentException();
        }

        public async Task<int> Like(Post post, AppUser user)
        {
            if (post != null && user != null)
            {
                if (!IsLiked(post, user))
                {
                    Like like = new Like
                    {
                        User = user,
                        Date = DateTime.Now
                    };
                    post.Likes.Add(like);
                    await unitOfWork.SaveAsync();
                }
                int numOfLikes = post.Likes.Count();
                return numOfLikes;
            }
            else throw new ArgumentNullException();
        }

        public async Task<int> Unlike(Post post, AppUser user)
        {
            if (post != null && user != null)
            {
                if (IsLiked(post, user))
                {
                    var like = GetLike(post, user);
                    unitOfWork.Likes.Delete(like);
                    await unitOfWork.SaveAsync();
                }
                int numOfLikes = post.Likes.Count();
                return numOfLikes;
            }
            else throw new ArgumentNullException();
        }

        public bool IsLiked(Post post, AppUser user)
        {
            if (post != null && user != null)
            {
                var checkLike = GetLike(post, user);

                if (checkLike == null)
                    return false;
                else
                    return true;
            }
            else throw new ArgumentNullException();
        }

        private Like GetLike(Post post, AppUser user)
        {
            var like = post.Likes.FirstOrDefault(l => l.User.Alias == user.Alias);
            return like;
        }
    }
}

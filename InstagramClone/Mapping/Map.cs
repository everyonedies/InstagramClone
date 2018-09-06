using InstagramClone.Domain.Models;
using InstagramClone.Models;
using System.Linq;

namespace InstagramClone.Mapping
{
    public static class Map
    {
        public static AppUserViewModel MapAppUser(this AppUser appUser)
        {
            AppUserViewModel userView = new AppUserViewModel
            {
                Alias = appUser.Alias,
                Bio = appUser.Bio,
                Name = appUser.UserName,
                RealName = appUser.RealName,
                WebSite = appUser.WebSite,
                Picture = appUser.Picture,
            };
            return userView;
        }

        public static PostViewModel MapPost(this Post post)
        {
            PostViewModel postView = new PostViewModel
            {
                Id = post.Id,
                Text = post.Text,
                Date = post.Date,
                PicturePreview = post.PicturePreview,
                PictureView = post.PictureView,
                User = post.User.MapAppUser(),
            };
            return postView;
        }

        public static LikeViewModel MapLike(this Like like)
        {
            LikeViewModel likeView = new LikeViewModel
            {
                Date = like.Date,
                Post = like.Post.MapPost(),
                User = like.User.MapAppUser(),
            };
            return likeView;
        }

        public static CommentViewModel MapComment(this Comment comment)
        {
            CommentViewModel commentView = new CommentViewModel
            {
                Date = comment.Date,
                Text = comment.Text,
                Post = comment.Post.MapPost(),
                User = comment.User.MapAppUser()
            };
            return commentView;
        }

        public static TagViewModel MapTag(this Tag tag)
        {
            TagViewModel tagView = new TagViewModel
            {
                Text = tag.Text
            };
            return tagView;
        }
    }
}

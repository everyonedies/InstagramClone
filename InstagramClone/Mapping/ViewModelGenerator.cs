using InstagramClone.Domain.Models;
using InstagramClone.Models;
using System.Collections.Generic;
using System.Linq;

namespace InstagramClone.Mapping
{
    public static class ViewModelGenerator
    {
        public static AppUserViewModel GetAppUserViewModel(this AppUser appUser)
        {
            var whoFollows = appUser.Followers?.Select(u => u.WhoFollows);
            var forWhomFollows = appUser.Following?.Select(u => u.ForWhomFollows);

            AppUserViewModel userViewModel = appUser.MapAppUser();

            userViewModel.NumberOfFollowers = whoFollows?.Count();
            userViewModel.NumberOfFollowing = forWhomFollows?.Count();

            userViewModel.Posts = appUser.Posts?.OrderByDescending(p => p.Date).ToList().GetPostsViewModel();

            return userViewModel;
        }

        public static ICollection<PostViewModel> GetPostsViewModel(this ICollection<Post> posts)
        {
            ICollection<PostViewModel> result = posts.Select(p =>
            {
                var res = p.MapPost();

                res.Likes = p.Likes?.Select(l => l.MapLike()).ToList();
                res.Comments = p.Comments?.Select(c => c.MapComment()).ToList();

                return res;
            }).ToList();

            return result;
        }

        public static PostViewModel GetPostViewModel(this Post post)
        {
            var comments = post.Comments?.Select(c =>
            {
                var commentMap = c.MapComment();
                commentMap.User = c.User.MapAppUser();
                return commentMap;
            }).ToList();

            var likes = post.Likes?.Select(l =>
            {
                var res = l.MapLike();
                res.User = l.User.MapAppUser();
                return res;
            }).ToList();

            var tags = post.TagPosts?.Select(tp =>
            {
                var res = tp.Tag.MapTag();
                return res;
            }).ToList();

            PostViewModel postViewModel = post.MapPost();
            postViewModel.Comments = comments;
            postViewModel.Likes = likes;
            postViewModel.Tags = tags;

            return postViewModel;
        }
    }
}

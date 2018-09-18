using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPostService postService;
        private readonly UserManager<AppUser> userManager;

        public PostController(IUnitOfWork unitOfWork, 
            IPostService postService,
            UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.postService = postService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> ShowPost(int id)
        {
            var post = unitOfWork.Posts.GetByIdWithItems(id);
            var user = await userManager.GetUserAsync(User);

            if (post != null)
            {
                if (user != null)
                {
                    var curUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);

                    if (postService.IsLiked(post, curUser))
                        ViewBag.LikeState = "Like";
                    else
                        ViewBag.LikeState = "Unlike";
                }

                var postView = GetPostViewModel(post);
                return View(postView);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewComment(int postId, string text)
        {
            var user = await userManager.GetUserAsync(User);
            bool result = postService.AddNewComment(postId, text, user.Alias);

            if (result)
                return Json(new { alias = user.Alias });
            else
                return Json(new { error = "Anon users can't send comments" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(int postId)
        {
            var user = await userManager.GetUserAsync(User);
            Post post = unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser curUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);

            bool result = false;
            string status = "";
            if (postService.IsLiked(post, curUser))
            {
                result = postService.Unlike(post, curUser);
                status = "Unlike";
            }
            else
            {
                result = postService.Like(post, curUser);
                status = "Like";
            }

            if (result)
            {
                int numOfLikes = post.Likes.Count();
                return Json(new { likes = numOfLikes, state = status });
            }
            else return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPostCaption(int postId, string caption)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                bool result = postService.AddPostCaption(postId, caption, user.Alias);
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            else
            {
                return Redirect("/post/" + postId);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPostTags(int postId, string tags)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                bool result = false;
                if (tags != null && tags != string.Empty)
                    result = postService.AddPostTags(postId, tags, user.Alias);
                else
                    result = postService.RemovePostTags(postId, user.Alias);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            else
            {
                return Redirect("/post/" + postId);
            }
        }

        private PostViewModel GetPostViewModel(Post post)
        {
            var comments = post.Comments.Select(c => 
            {
                var commentMap = c.MapComment();
                commentMap.User = c.User.MapAppUser();
                return commentMap;
            }).ToList();

            var likes = post.Likes.Select(l => 
            {
                var res = l.MapLike();
                res.User = l.User.MapAppUser();
                return res;
            }).ToList();

            var tags = post.TagPosts.Select(tp =>
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
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InstagramClone.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPostService postService;
        private readonly UserManager<AppUser> userManager;

        public PostController(IUnitOfWork unitOfWork, IPostService postService, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.postService = postService;
            this.userManager = userManager;
        }

        public IActionResult ShowPost(int id)
        {
            var post = unitOfWork.Posts.GetByIdWithItems(id);
            var postView = GetPostViewModel(post);

            return View(postView);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddNewComment(int postId, string text)
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool result = postService.AddNewComment(postId, text, user.Alias);
                if (result)
                    return Json(new { alias = user.Alias });
                else
                    return Json(new { error = "Anon users can't send comments" });
            }
            else
            {
                return Redirect("/post/" + postId);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPostCaption(int postId, string caption)
        {
            var user = userManager.GetUserAsync(User).Result;
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

            PostViewModel postViewModel = post.MapPost();
            postViewModel.Comments = comments;
            postViewModel.Likes = likes;

            return postViewModel;
        }
    }
}
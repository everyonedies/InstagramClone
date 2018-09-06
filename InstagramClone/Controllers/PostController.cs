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
                if (postId == 0 || text == null || text == "")
                {
                    return Content(postId + " " + text);
                }
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

        private PostViewModel GetPostViewModel(Post post)
        {
            var comments = post.Comments.Select(c => 
            {
                var commentMap = c.MapComment();
                commentMap.User = c.User.MapAppUser();
                return commentMap;
            }).ToList();

            PostViewModel postViewModel = post.MapPost();
            postViewModel.Comments = comments;

            return postViewModel;
        }
    }
}
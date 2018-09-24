using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    [Authorize(Roles = "admin, moder, user")]
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

        [AllowAnonymous]
        public async Task<IActionResult> ShowPost(int id)
        {
            Post post = await unitOfWork.Posts.GetByIdWithItems(id);
            AppUser user = await userManager.GetUserAsync(User);

            if (post != null)
            {
                if (user != null)
                {
                    AppUser curUser = await unitOfWork.Users.GetByAliasWithItems(user.Alias);

                    if (postService.IsLiked(post, curUser))
                        ViewBag.LikeState = "Like";
                    else
                        ViewBag.LikeState = "Unlike";
                }

                PostViewModel postView = post.GetPostViewModel();
                return View(postView);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment(int postId, string text)
        {
            AppUser user = await userManager.GetUserAsync(User);
            try
            {
                await postService.AddNewComment(postId, text, user.Alias);
                return Json(new { alias = user.Alias });
            }
            catch (ArgumentException)
            {
                return Json(new { error = "Anon users can't send comments" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postId)
        {
            AppUser user = await userManager.GetUserAsync(User);
            Post post = await unitOfWork.Posts.GetByIdWithItems(postId);
            AppUser curUser = await unitOfWork.Users.GetByAliasWithItems(user.Alias);

            try
            {
                int numOfLikes = 0;
                string status = "";

                if (postService.IsLiked(post, curUser))
                {
                    numOfLikes = await postService.Unlike(post, curUser);
                    status = "Unlike";
                }
                else
                {
                    numOfLikes = await postService.Like(post, curUser);
                    status = "Like";
                }

                return Json(new { likes = numOfLikes, state = status });
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCaption(int postId, string caption)
        {
            AppUser user = await userManager.GetUserAsync(User);

            try
            {
                await postService.AddPostCaption(postId, caption, user.Alias);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPostTags(int postId, string tags)
        {
            AppUser user = await userManager.GetUserAsync(User);

            try
            {
                if (tags != null && tags != string.Empty)
                    await postService.AddPostTags(postId, tags, user.Alias);
                else
                    await postService.RemovePostTags(postId, user.Alias);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
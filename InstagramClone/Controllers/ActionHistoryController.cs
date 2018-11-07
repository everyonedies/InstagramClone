using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    public class ActionHistoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;

        public ActionHistoryController(IUnitOfWork unitOfWork, IUserService userService, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> GetLikedPosts()
        {
            AppUser currentUser = await userManager.GetUserAsync(User);

            ICollection<Post> likedPosts = await userService.GetUserLikedPosts(currentUser.Alias);
            ICollection<PostViewModel> likedPostsViewModel = likedPosts.GetPostsViewModel();

            return View(likedPostsViewModel);
        }
    }
}
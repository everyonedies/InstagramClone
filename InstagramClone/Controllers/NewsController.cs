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
    [Authorize(Roles = "admin, moder, user")]
    public class NewsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;

        public NewsController(IUnitOfWork unitOfWork, IUserService userService, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetUserNews()
        {
            AppUser appUser = await userManager.GetUserAsync(User);
            ICollection<Post> userNews = await userService.GetUserNews(appUser.Alias);

            ICollection<PostViewModel> viewModel = userNews.GetPostsViewModel();
            return View(viewModel);
        }
    }
}
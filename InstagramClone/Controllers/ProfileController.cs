using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InstagramClone.Models;
using InstagramClone.Mapping;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using InstagramClone.Domain.Interfaces;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProfileService profileService;
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;

        public ProfileController(IUnitOfWork unitOfWork, 
            IProfileService profileService, 
            IUserService userService, 
            UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.profileService = profileService;
            this.userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetProfilePicture()
        {
            AppUser user = await userManager.GetUserAsync(User);
            IFormFile file = HttpContext.Request.Form.Files.First();
            string type = file.ContentType;

            if (type == "image/jpeg" || type == "image/gif" || type == "image/png")
            {
                Image image = Image.FromStream(file.OpenReadStream(), true, false);
                string imageExt = Path.GetExtension(file.FileName);
                profileService.SetProfilePhoto(user, image, imageExt);
            }

            return Redirect("/" + user.Alias);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewPost()
        {
            AppUser user = await userManager.GetUserAsync(User);
            IFormFile file = HttpContext.Request.Form.Files.First();
            string type = file.ContentType;

            if (type == "image/jpeg" || type == "image/gif" || type == "image/png")
            {
                Image image = Image.FromStream(file.OpenReadStream(), true, false);
                string imageExt = Path.GetExtension(file.FileName);
                profileService.AddNewPost(user, image, imageExt);
            }

            return Redirect("/" + user.Alias);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(int postId)
        {
            AppUser user = await userManager.GetUserAsync(User);
            bool result = profileService.DeletePost(user, postId);

            if (result)
                return Redirect("/" + user.Alias);
            else
                return BadRequest();
        }

        public async Task<IActionResult> GetFollowers(string alias)
        {
            AppUser appUser = await userManager.GetUserAsync(User);
            var userFollowers = userService.GetUserFollowers(appUser).Select(u => u.Alias);

            if (userFollowers.Count() != 0)
                return Json(userFollowers);
            else
                return Json(new { error = $"The user '{alias}' doesn't have followers" });
        }

        public async Task<IActionResult> GetFollowing(string alias)
        {
            AppUser appUser = await userManager.GetUserAsync(User);
            var userFollowing = userService.GetUserFollowing(appUser).Select(u => u.Alias);

            if (userFollowing.Count() != 0)
                return Json(userFollowing);
            else
                return Json(new { error = $"The user '{alias}' doesn't following anyone"});
        }

        public async Task<IActionResult> GetUserProfile(string alias)
        {
            AppUser user = await userManager.GetUserAsync(User);
            AppUser targetUser = unitOfWork.Users.GetByAliasWithItems(alias);

            if (targetUser == null)
                return View();

            if (user != null)
            {
                AppUser currentUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);
                if (currentUser.Alias != targetUser.Alias)
                {
                    bool isFollowing = userService.IsUserFollowing(currentUser, targetUser);
                    if (isFollowing)
                        ViewBag.Following = "Unfollow";
                    else
                        ViewBag.Following = "Follow";
                }
            }
            else
            {
                ViewBag.Following = "Anon";
            }

            AppUserViewModel userViewModel = targetUser.GetAppUserViewModel();
            return View(userViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string alias)
        {
            AppUser user = await userManager.GetUserAsync(User);
            AppUser targetUser = unitOfWork.Users.GetByAliasWithItems(alias);

            if (targetUser != null)
            {
                AppUser currentUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);

                if (currentUser.Alias != targetUser.Alias)
                {
                    var isFollowing = userService.IsUserFollowing(currentUser, targetUser);

                    if (isFollowing)
                        userService.Unfollow(currentUser, targetUser);
                    else
                        userService.Follow(currentUser, targetUser);
                }
            }

            return Redirect("/" + alias);
        }
    }
}
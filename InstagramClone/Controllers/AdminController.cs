using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    [Authorize(Roles = "moder, admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAdminService adminService;
        private readonly UserManager<AppUser> userManager;

        public AdminController(IUnitOfWork unitOfWork, IAdminService adminService, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.adminService = adminService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllUsers()
        {
            List<AppUser> allUsers = await unitOfWork.Users.ListAllAsync();
            List<AppUserViewModel> allUsersViewModel = allUsers.Select(u => u.GetAppUserViewModel()).ToList();

            return View(allUsersViewModel);
        }

        public async Task<IActionResult> GetUser(string alias)
        {
            AppUser currentUser = await userManager.GetUserAsync(User);
            AppUser targetUser = await unitOfWork.Users.GetByAliasWithItems(alias);
            AppUserViewModel targetUserViewModel = targetUser.GetAppUserViewModel();

            if (currentUser.Alias != targetUser.Alias)
            {
                bool currUserM = await userManager.IsInRoleAsync(currentUser, "moder");
                bool currUserA = await userManager.IsInRoleAsync(currentUser, "admin");
                bool appUserM = await userManager.IsInRoleAsync(targetUser, "moder");
                bool appUserA = await userManager.IsInRoleAsync(targetUser, "admin");

                if (currUserA && appUserA || currUserM && appUserA || currUserM && appUserM)
                    ViewBag.Permission = false;
                else
                    ViewBag.Permission = true;
            }
            else
            {
                ViewBag.Permission = true;
            }

            TempData["returnUrl"] = HttpContext.Request.Path.ToString() + "?alias=" +  alias;

            return View(targetUserViewModel);
        }

        public IActionResult BlockUser(string alias)
        {
            return View();
        }

        public IActionResult UnblockUser(string alias)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                AppUser user = await userManager.GetUserAsync(User);
                await adminService.DeletePost(user, postId);

                string returnUrl = Convert.ToString(TempData["returnUrl"]);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return Redirect("/Admin/GetAllUsers");
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult DeleteComment(int commentId)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult SetModerRoleForUser(string alias)
        {
            return View();
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult UnsetModerRoleForUser(string alias)
        {
            return View();
        }
    }
}
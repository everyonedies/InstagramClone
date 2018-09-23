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

        public IActionResult GetAllPosts()
        {
            return View();
        }

        public IActionResult GetAllComments()
        {
            return View();
        }

        public IActionResult GetAllTags()
        {
            return View();
        }

        public IActionResult GetUser(string alias)
        {
            AppUser appUser = unitOfWork.Users.GetByAliasWithItems(alias);
            AppUserViewModel appUserViewModel = appUser.GetAppUserViewModel();
            TempData["returnUrl"] = HttpContext.Request.Path.ToString() + "?alias=" +  alias;

            return View(appUserViewModel);
        }

        public IActionResult BanUser(string alias)
        {
            return View();
        }

        public IActionResult UnbanUser(string alias)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                adminService.DeletePost(postId);
                string returnUrl = Convert.ToString(TempData["returnUrl"]);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return Redirect("/Admin/GetAllUsers");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult DeleteComment(int commentId)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult SetModerRole(string alias)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult UnsetModerRole(string alias)
        {
            return View();
        }
    }
}
﻿using InstagramClone.Domain.Models;
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
        public IActionResult SetProfilePicture()
        {
            var user = userManager.GetUserAsync(User).Result;

            var file = HttpContext.Request.Form.Files.First();
            Image image = Image.FromStream(file.OpenReadStream(), true, false);
            string imageExt = Path.GetExtension(file.FileName);

            profileService.SetProfilePhoto(user, image, imageExt);

            return Redirect(user.Alias);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddNewPost()
        {
            var user = userManager.GetUserAsync(User).Result;

            var file = HttpContext.Request.Form.Files.First();
            Image image = Image.FromStream(file.OpenReadStream(), true, false);
            string imageExt = Path.GetExtension(file.FileName);

            profileService.AddNewPost(user, image, imageExt);

            return Json(new { alias = user.Alias });
        }

        public IActionResult GetFollowers(string alias)
        {
            var userFollowers = userService.GetUserFollowers(alias);

            if (userFollowers.Count() != 0)
            {
                return Json(userFollowers);
            }
            else
            {
                return Json(new { error = $"The user '{alias}' doesn't have followers" });
            }
        }

        public IActionResult GetFollowing(string alias)
        {
            var userFollowing = userService.GetUserFollowing(alias);

            if (userFollowing.Count() != 0)
            {
                return Json(userFollowing);
            }
            else
            {
                return Json(new { error = $"The user '{alias}' doesn't following anyone"});
            }
        }

        public IActionResult GetUserProfile(string alias)
        {
            var user = userManager.GetUserAsync(User).Result;
            var targetUser = unitOfWork.Users.GetByAliasWithItems(alias);

            if (targetUser == null)
                return View();

            if (user != null)
            {
                var currentUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);

                if (currentUser.Alias != targetUser.Alias)
                {
                    var isFollowing = userService.IsUserFollowing(currentUser, targetUser);
                    if (isFollowing)
                        ViewBag.Following = "Unfollow";
                    else
                        ViewBag.Following = "Follow";
                }
            }
            else
                ViewBag.Following = "Anon";

            AppUserViewModel userViewModel = GetAppUserViewModel(targetUser);
            return View(userViewModel);
        }

        [Authorize]
        public IActionResult Follow(string alias)
        {
            var user = userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                var currentUser = unitOfWork.Users.GetByAliasWithItems(user.Alias);
                var targetUser = unitOfWork.Users.GetByAliasWithItems(alias);

                if (currentUser.Alias != targetUser.Alias)
                {
                    var isFollowing = userService.IsUserFollowing(currentUser, targetUser);

                    if (isFollowing)
                    {
                        userService.Unfollow(currentUser, targetUser);
                    }
                    else
                    {
                        userService.Follow(currentUser, targetUser);
                    }
                }
            }

            return Redirect("/" + alias);
        }

        private AppUserViewModel GetAppUserViewModel(AppUser appUser)
        {
            var whoFollows = appUser.Followers.Select(u => u.WhoFollows);
            var forWhomFollows = appUser.Following.Select(u => u.ForWhomFollows);

            AppUserViewModel userViewModel = appUser.MapAppUser();

            userViewModel.NumberOfFollowers = whoFollows.Count();
            userViewModel.NumberOfFollowing = forWhomFollows.Count();

            userViewModel.Posts = appUser.Posts.Select(p => 
            {
                var res = p.MapPost();

                res.Likes = p.Likes.Select(l => l.MapLike()).ToList();
                res.Comments = p.Comments.Select(c => c.MapComment()).ToList();

                return res;
            }).ToList();

            return userViewModel;
        }
    }
}
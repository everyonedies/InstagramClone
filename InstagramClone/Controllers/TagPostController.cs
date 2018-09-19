using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InstagramClone.Controllers
{
    public class TagPostController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TagPostController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult ShowPostsByTag(string text)
        {
            Tag tag = new Tag { Text = text };
            ICollection<Post> posts = unitOfWork.Posts.GetPostsWithItemsByTag(tag);

            ViewBag.Tag = text;
            ICollection<PostViewModel> viewModel = posts.GetPostsViewModel();

            return View(viewModel);
        }
    }
}
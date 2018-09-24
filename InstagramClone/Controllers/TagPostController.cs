using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    [AllowAnonymous]
    public class TagPostController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TagPostController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> ShowPostsByTag(string text)
        {
            Tag tag = new Tag { Text = text };
            ICollection<Post> posts = await unitOfWork.Posts.GetPostsWithItemsByTag(tag);

            ViewBag.Tag = text;
            ICollection<PostViewModel> viewModel = posts.GetPostsViewModel();

            return View(viewModel);
        }
    }
}
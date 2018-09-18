using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using InstagramClone.Mapping;
using InstagramClone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
            var posts = unitOfWork.Posts.GetPostsWithItemsByTag(tag);

            ViewBag.Tag = text;
            var viewModel = GetTagPostViewModel(posts);

            return View(viewModel);
        }

        private ICollection<PostViewModel> GetTagPostViewModel(ICollection<Post> posts)
        {
            var viewModel = posts.Select(p =>
            {
                var res = p.MapPost();

                res.Likes = p.Likes.Select(l => l.MapLike()).ToList();
                res.Comments = p.Comments.Select(c => c.MapComment()).ToList();

                return res;
            }).ToList();

            return viewModel;
        }
    }
}
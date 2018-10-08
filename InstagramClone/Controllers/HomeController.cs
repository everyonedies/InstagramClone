using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InstagramClone.Domain.Interfaces;
using System.Collections.Generic;
using InstagramClone.Domain.Models;
using System.Threading.Tasks;
using InstagramClone.Models;
using InstagramClone.Mapping;
using Microsoft.AspNetCore.Authorization;

namespace InstagramClone.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUserService userService, IUnitOfWork unitOfWork)
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int numOfTopUsers = 20)
        {
            ICollection<AppUser> topUsers = await userService.GetTopUsers(numOfTopUsers);
            ICollection<AppUserViewModel> topUsersViewModel = topUsers.Select(u => u.GetAppUserViewModel()).ToList();

            return View(topUsersViewModel);
        }

        public async Task<IActionResult> SearchAjax(string text)
        {
            object data;
            int len = 0;
            if (text[0] == '#')
            {
                ICollection<Tag> tags = await unitOfWork.Tags.GetTagsByNameWithItems(text.Substring(1));
                var res = tags.Select(t => new { text = t.Text, type = "tag" });
                len = res.Count();
                data = res;
            }
            else if (text[0] == '@')
            {
                ICollection<AppUser> users = await userService.FindUsersByAlias(text.Substring(1));
                var res = users.Select(u => new { text = u.Alias, type = "user" });
                len = res.Count();
                data = res;
            }
            else
            {
                ICollection<Tag> tags = await unitOfWork.Tags.GetTagsByNameWithItems(text);
                var tagsText = tags.Select(t => new { text = t.Text, type = "tag" });

                ICollection<AppUser> users = await userService.FindUsersByAlias(text);
                var userAlias = users.Select(u => new { text = u.Alias, type = "user" });

                var res = userAlias.Concat(tagsText).OrderBy(i => i.text);
                len = res.Count();
                data = res;
            }

            if (len != 0)
                return Json(data);
            else
                return Json(new { error = "Not found" });
        }
    }
}

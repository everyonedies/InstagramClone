using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InstagramClone.Domain.Interfaces;

namespace InstagramClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUserService userService, IUnitOfWork unitOfWork)
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchAjax(string text)
        {
            object data;
            int len = 0;
            if (text[0] == '#')
            {
                var res = unitOfWork.Tags.GetTagsByNameWithItems(text.Substring(1)).Select(t => new { text = t.Text, type = "tag" });
                len = res.Count();
                data = res;
            }
            else if (text[0] == '@')
            {
                var res = userService.FindUsersByAlias(text.Substring(1)).Select(u => new { text = u, type = "user" });
                len = res.Count();
                data = res;
            }
            else
            {
                var tags = unitOfWork.Tags.GetTagsByNameWithItems(text).Select(t => new { text = t.Text, type = "tag" });
                var users = userService.FindUsersByAlias(text).Select(u => new { text = u, type = "user" });
                var res = users.Concat(tags).OrderBy(i => i.text);
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

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InstagramClone.Domain.Interfaces;

namespace InstagramClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchAjax(string alias)
        {
            var list = userService.FindUsersByAlias(alias);

            if (list.Count() != 0)
            {
                return Json(list);
            }
            else
            {
                return Json(new { error = "Not found" });
            }
        }
    }
}

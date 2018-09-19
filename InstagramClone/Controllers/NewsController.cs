using Microsoft.AspNetCore.Mvc;

namespace InstagramClone.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
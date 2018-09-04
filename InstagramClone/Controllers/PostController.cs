using Microsoft.AspNetCore.Mvc;

namespace InstagramClone.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
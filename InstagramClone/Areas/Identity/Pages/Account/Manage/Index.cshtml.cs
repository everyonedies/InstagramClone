using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstagramClone.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Alias:")]
            public string Alias { get; set; }

            [Display(Name = "Name:")]
            public string RealName { get; set; }

            [Display(Name = "Bio:")]
            public string Bio { get; set; }

            [Display(Name = "Web-site:")]
            [Url]
            public string WebSite { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
                Alias = user.Alias,
                WebSite = user.WebSite,
                Bio = user.Bio,
                RealName = user.RealName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.Alias != Input.Alias)
            {
                var usr = _unitOfWork.Users.GetByAliasWithItems(Input.Alias);
                if (usr == null)
                {
                    user.Alias = Input.Alias;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"The alias '{Input.Alias}' already exists.");
                    return Page();
                }
            }

            user.Bio = Input.Bio;
            user.WebSite = Input.WebSite;
            user.RealName = Input.RealName;

            await _unitOfWork.SaveAsync();

            return Redirect("/profile");
        }
    }
}
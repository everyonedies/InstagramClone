using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InstagramClone.Domain.Interfaces;
using InstagramClone.Domain.Models;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            IHostingEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
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
            [RegularExpression(@"^[a-z0-9_]+$", ErrorMessage = "Alias must contains only numbers, underscore and latin letters in lower case")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Alias must be at least 1 characters")]
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
            AppUser user = await _userManager.GetUserAsync(User);
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

            AppUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.Alias != Input.Alias)
            {
                AppUser usr = await _unitOfWork.Users.GetByAliasWithItems(Input.Alias);
                if (usr == null)
                {
                    user = await _unitOfWork.Users.GetByAliasWithItems(user.Alias);
                    user.Picture = user.Picture.Replace(user.Alias, Input.Alias);
                    foreach (var i in user.Posts)
                    {
                        i.PicturePreview = i.PicturePreview.Replace(user.Alias, Input.Alias);
                        i.PictureView = i.PicturePreview.Replace(user.Alias, Input.Alias);
                    }
                    string oldDir = Path.Combine(_hostingEnvironment.WebRootPath, $"images\\Users\\{user.Alias}\\");
                    string newDir = Path.Combine(_hostingEnvironment.WebRootPath, $"images\\Users\\{Input.Alias}\\");
                    Directory.Move(oldDir, newDir);

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

            return Redirect("/" + user.Alias);
        }
    }
}
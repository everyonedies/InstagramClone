using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using InstagramClone.Domain.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using InstagramClone.Domain.Interfaces;

namespace InstagramClone.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IHostingEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Alias")]
            [RegularExpression(@"^[a-z0-9_]+$", ErrorMessage = "Alias must contains only numbers, underscore and latin letters in lower case.")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Alias must be at least 1 characters.")]
            public string Alias { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = Input.Name, Alias = Input.Alias, Picture = "/images/profile.jpg" };

                bool flag = false;
                AppUser usrName = _unitOfWork.Users.List(u => u.UserName == Input.Name).FirstOrDefault();

                if (usrName == null)
                {
                    user.UserName = Input.Name;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"The name '{Input.Name}' already exists.");
                    flag = true;
                }

                AppUser usrAlias = _unitOfWork.Users.List(u => u.Alias == Input.Alias).FirstOrDefault();
                if (usrAlias == null)
                {
                    user.Alias = Input.Alias;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"The alias '{Input.Alias}' already exists.");
                    flag = true;
                }

                if (flag)
                {
                    return Page();
                }

                string path = _hostingEnvironment.WebRootPath + $@"\images\Users\{Input.Alias}";
                Directory.CreateDirectory(path);

                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

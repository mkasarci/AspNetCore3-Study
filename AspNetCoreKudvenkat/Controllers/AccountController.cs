using System.Threading.Tasks;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser{ UserName = registerViewModel.Email, Email = registerViewModel.Email};
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, registerViewModel.Password, false, true);    
                    return RedirectToAction("Index","Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError(string.Empty, "An error occured.");
            return View(registerViewModel);
        }
    }
}
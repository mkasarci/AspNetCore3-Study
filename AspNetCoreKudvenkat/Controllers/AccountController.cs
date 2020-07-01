using System.Threading.Tasks;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return Json(true);
            }
            
            return Json($"Email {email} is already in use!");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                { 
                    UserName = registerViewModel.Email,
                     Email = registerViewModel.Email,
                      City = registerViewModel.City
                };
                
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, true);
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too many failed login attempts! Wait for a while to login again");
                    return View();
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "This account is suspended. Please contact with the Customer Support.");
                    return View();
                }
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(ReturnUrl))
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return LocalRedirect(ReturnUrl); // if it isn't a local URL throws an exception.
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else{ return View(); }

            ModelState.AddModelError(string.Empty, "Wrong username or password!");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
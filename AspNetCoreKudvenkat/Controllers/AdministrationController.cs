using System.Threading.Tasks;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> isValidName(string RoleName)
        {
            var identityRole = await _roleManager.FindByNameAsync(RoleName);
            if(identityRole is null)
            {
                return Json(true);
            }

            return Json($"Role name {RoleName} already in use! Please select another name.");

        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleViewModel.RoleName
                };
                var result = await _roleManager.CreateAsync(identityRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
                {   
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError(string.Empty, "An error occured!");
            return View();
        }
    }
}
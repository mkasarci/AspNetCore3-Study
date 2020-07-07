using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreKudvenkat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> isValidName(string RoleName)
        {
            var identityRole = await _roleManager.FindByNameAsync(RoleName);
            if (identityRole is null)
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
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError(string.Empty, "An error occured!");
            return View();
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                ModelState.AddModelError(string.Empty, "Couldn't find any role with that ID. Please contact with support team.");
                return View();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var users = usersInRole.Select(u => u.Email);

            EditRoleViewModel model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = users.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);
                role.Name = editRoleViewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "An error occured!");
            return View(editRoleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "RoleId is empty. Please contact with support.";
                return View("Error/NotFound");
            }

            ViewBag.roleId = id;

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found.";
                return View("Error/NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.Email
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string roleId, List<UserRoleViewModel> userRoleViewModels)
        {
            if (roleId == null)
            {
                ViewBag.ErrorMessage = "RoleId is empty. Please contact with support.";
                return View("Error/NotFound");
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            foreach (var user in userRoleViewModels)
            {
                var applicationUser = await _userManager.FindByIdAsync(user.UserId);

                IdentityResult result = null;

                if (user.IsSelected && !(await _userManager.IsInRoleAsync(applicationUser, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(applicationUser, role.Name);
                }
                else if (!user.IsSelected && (await _userManager.IsInRoleAsync(applicationUser, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(applicationUser, role.Name);

                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    continue;
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            return RedirectToAction("EditRole", "Administration", new {id = roleId});
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with ID = {Id} cannot be found!";
                return View("Error/NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

                if (user is null)
                {
                    ViewBag.ErrorMessage = $"User with ID = {editUserViewModel.Id} cannot be found!";
                    return View("Error/NotFound");
                }

                user.UserName = editUserViewModel.UserName;
                user.Email = editUserViewModel.Email;
                user.City = editUserViewModel.City;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ModelState.AddModelError(string.Empty, "An error occured!");
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user is null)
            {
                ViewBag.ErrorMessage($"User with ID = {id} cannot be found!");
                return View("Error/NotFound");
            }

            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                ViewBag.ErrorMessage($"Role with ID = {id} cannot be found!");
                return View("Error/NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("ListRoles");
        }
    }
}
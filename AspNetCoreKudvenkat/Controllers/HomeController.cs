using AspNetCoreKudvenkat.Models;
using AspNetCoreKudvenkat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspNetCoreKudvenkat.Controllers
{
    [Route("[controller]/[action]")] //[controller] token gets the Controller name.
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("~/")]
        [Route("~/Home")]
        [Route("")]
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }
        [Route("{id?}")]
        public IActionResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Details Page"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee addedEmployee = _employeeRepository.Add(employee);
                return RedirectToAction("Details","Home", new{ id= addedEmployee.Id });
            }
            ModelState.AddModelError(string.Empty, "An error accured!");
            return View();
        }
    }
}
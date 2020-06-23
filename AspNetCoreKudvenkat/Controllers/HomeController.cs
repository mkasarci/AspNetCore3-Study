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
    }
}
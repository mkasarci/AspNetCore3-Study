using AspNetCoreKudvenkat.Models;
using AspNetCoreKudvenkat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AspNetCoreKudvenkat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEmployeeRepository EmployeeRepository { get; }

        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }
        public IActionResult Details()
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            homeDetailsViewModel.Employee = _employeeRepository.GetEmployee(1);
            homeDetailsViewModel.PageTitle = "Details Page";
            return View(homeDetailsViewModel);
        }
    }
}
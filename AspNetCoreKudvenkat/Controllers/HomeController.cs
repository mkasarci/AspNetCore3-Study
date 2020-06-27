using AspNetCoreKudvenkat.Models;
using AspNetCoreKudvenkat.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;

namespace AspNetCoreKudvenkat.Controllers
{
    [Route("[controller]/[action]")] //[controller] token gets the Controller name.
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, 
                              IWebHostEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
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
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images"); //wwwroot folder path + /images
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName; 
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                Employee addedEmployee = _employeeRepository.Add(newEmployee);
                return RedirectToAction("Details","Home", new{ id= addedEmployee.Id });
            }
            ModelState.AddModelError(string.Empty, "An error accured!");
            return View();
        }
    }
}
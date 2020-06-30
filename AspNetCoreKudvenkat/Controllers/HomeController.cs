using AspNetCoreKudvenkat.Models;
using AspNetCoreKudvenkat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public HomeController(IEmployeeRepository employeeRepository, 
                              IWebHostEnvironment hostingEnvironment,
                              ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        [Route("~/")]
        [Route("~/Home")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }
        [Route("{id?}")]
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            //throw new Exception("Test error");
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");

            if (id is null) 
            {
                id = 1;
            } 

            Employee employee = _employeeRepository.GetEmployee(id.Value);

            if (employee is null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = employee,
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
                string uniqueFileName = ProcessUploadedFile(model);

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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee =_employeeRepository.GetEmployee(id);
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        {
            if (ModelState.IsValid) 
            {

                Employee employee = new Employee
                {
                    Id = employeeEditViewModel.Id,
                    Name = employeeEditViewModel.Name,
                    Email = employeeEditViewModel.Email,
                    Department = employeeEditViewModel.Department,
                    PhotoPath = employeeEditViewModel.ExistingPhotoPath
                };
                if (employeeEditViewModel.Photo != null)
                {
                    if (employeeEditViewModel.ExistingPhotoPath != null)
                    {
                        string oldPhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", employeeEditViewModel.ExistingPhotoPath);
                        System.IO.File.Delete(oldPhotoPath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(employeeEditViewModel);
                }

                _employeeRepository.Update(employee);

                return RedirectToAction("Details","Home",new{Id= employeeEditViewModel.Id});
            }
            ModelState.AddModelError(string.Empty, "An error accured!");
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel employeeEditViewModel)
        {
            string uniqueFileName = null;
                if (employeeEditViewModel.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images"); //wwwroot folder path + /images
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + employeeEditViewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        employeeEditViewModel.Photo.CopyTo(fileStream);
                    }
                }
            return uniqueFileName;
        }
    }
}
using AspNetCoreKudvenkat.Models;

namespace AspNetCoreKudvenkat.Controllers
{
    public class HomeController
    {
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEmployeeRepository EmployeeRepository { get; }

        public string Index()
        {
            return _employeeRepository.GetEmployee(1).Name;
        }
    }
}
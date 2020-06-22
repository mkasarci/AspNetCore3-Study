using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreKudvenkat.Models
{
    class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>{
                new Employee(){Id = 1, Name="Muhammet", Department="IT", Email="muhammed.kasarci@gmail.com"},
                new Employee(){Id = 2, Name="Ömer Faruk", Department="Management", Email="muhammed.kasarci@gmail.com"},
                new Employee(){Id = 3, Name="Furkan", Department="IT", Email="muhammed.kasarci@gmail.com"}
            };
        }

        public Employee GetEmployee(int Id) => _employeeList.FirstOrDefault(e => e.Id == Id);
    }
}
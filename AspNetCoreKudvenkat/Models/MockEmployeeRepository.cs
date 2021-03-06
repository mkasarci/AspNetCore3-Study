using System;
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
                new Employee(){Id = 1, Name="Muhammet", Department=Department.IT, Email="muhammed.kasarci@gmail.com"},
                new Employee(){Id = 2, Name="Ömer Faruk", Department=Department.HR, Email="muhammed.kasarci@gmail.com"},
                new Employee(){Id = 3, Name="Furkan", Department=Department.Management, Email="muhammed.kasarci@gmail.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee deletedEmployee = _employeeList.FirstOrDefault(e => e.Id == Id);
            if (deletedEmployee is null)
            {
                throw new ArgumentNullException();
            }
            _employeeList.Remove(deletedEmployee);
            return deletedEmployee;
         }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id) => _employeeList.FirstOrDefault(e => e.Id == Id);

        public Employee Update(Employee employee)
        {
            Employee updatedEmployee = _employeeList.FirstOrDefault(e => e.Id == employee.Id);
            if (updatedEmployee is null)
            {
                throw new ArgumentNullException();
            }
            updatedEmployee.Name = employee.Name;
            updatedEmployee.Email = employee.Email;
            updatedEmployee.Department = employee.Department;      
            return updatedEmployee;          
        }

    }
}
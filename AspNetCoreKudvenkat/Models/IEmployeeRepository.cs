using System.Collections.Generic;

namespace AspNetCoreKudvenkat.Models{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id); 
        IEnumerable<Employee> GetAllEmployee();
        Employee Add(Employee employee);
    }
}
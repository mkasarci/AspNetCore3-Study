using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace AspNetCoreKudvenkat.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SQLEmployeeRepository> _logger;

        public SQLEmployeeRepository(AppDbContext context,
                                    ILogger<SQLEmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Employee Add(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee deletedEmployee = _context.Employees.Find(Id);
            if (deletedEmployee != null)
            {    
                _context.Remove(deletedEmployee);
                _context.SaveChanges();
            }
            return deletedEmployee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _context.Employees;   
        }

        public Employee GetEmployee(int Id)
        {
            _logger.LogInformation("Get Employee");
            return _context.Employees.Find(Id);
        }

        public Employee Update(Employee employee)
        {
            _context.Update(employee);
            _context.SaveChanges();
            return employee;
        }
    }
}
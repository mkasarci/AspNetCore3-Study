using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCoreKudvenkat.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public SQLEmployeeRepository(AppDbContext context)
        {
            _context = context;
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
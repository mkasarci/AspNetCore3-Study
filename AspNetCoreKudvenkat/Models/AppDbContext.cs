using Microsoft.EntityFrameworkCore;

namespace AspNetCoreKudvenkat.Models
{
    public class AppDbContext : DbContext
    {
        private readonly DbContextOptions<AppDbContext> options;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }  

        public DbSet<Employee> Employees { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee{Id=1, Name="Ömer Faruk Kasarcı", Department = Department.Management, Email="ofk@yahoo.com"},
                new Employee{Id=2, Name="Furkan Kasarcı", Department = Department.Management, Email="furkankasarci@gmail.com"}
            );
        }  
    }
}
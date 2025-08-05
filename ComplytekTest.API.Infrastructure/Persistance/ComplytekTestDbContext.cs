using ComplytekTest.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComplytekTest.API.Infrastructure.Persistance
{
    public class ComplytekTestDbContext : DbContext
    {
        public ComplytekTestDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComplytekTestDbContext).Assembly);
        }
    }
}

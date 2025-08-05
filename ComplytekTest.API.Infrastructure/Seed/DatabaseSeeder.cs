using ComplytekTest.API.Core.Entities;
using ComplytekTest.API.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ComplytekTest.API.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ComplytekTestDbContext context)
        {
            if (!context.Departments.Any())
            {
                var departments = new List<Department>
            {
                new Department { Name = "Engineering", OfficeLocation = "Nairobi" },
                new Department { Name = "Human Resources", OfficeLocation = "Nakuru" },
                new Department { Name = "Finance", OfficeLocation = "Eldoret" }
            };

                await context.Departments.AddRangeAsync(departments);
                await context.SaveChangesAsync();
            }

            if (!context.Employees.Any())
            {
                var engineeringDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Engineering");
                var hrDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Human Resources");

                if (engineeringDept is null || hrDept is null)
                    throw new InvalidOperationException("Required departments not found.");

                var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Alice",
                    LastName = "Kariuki",
                    Email = "alice@company.com",
                    Salary = 60000,
                    DepartmentId = engineeringDept.Id
                },
                new Employee
                {
                    FirstName = "Gilbert",
                    LastName = "Kibet",
                    Email = "gilbert@company.com",
                    Salary = 50000,
                    DepartmentId = hrDept.Id
                }
            };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }
        }
    }

}

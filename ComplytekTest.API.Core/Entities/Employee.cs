using System.ComponentModel.DataAnnotations;

namespace ComplytekTest.API.Core.Entities
{
    public class Employee
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace ComplytekTest.API.Core.Entities
{
    public class Project
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        [Required]
        [StringLength(50)]
        public required string ProjectCode { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}

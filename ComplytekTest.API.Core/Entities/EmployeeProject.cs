using System.ComponentModel.DataAnnotations;

namespace ComplytekTest.API.Core.Entities
{
    public class EmployeeProject
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long ProjectId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Role { get; set; }

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;

        public virtual Employee Employee { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace ComplytekTest.API.Application.DTOs.Project
{
    public class ProjectToUpdateDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }

        public long DepartmentId { get; set; }
    }
}

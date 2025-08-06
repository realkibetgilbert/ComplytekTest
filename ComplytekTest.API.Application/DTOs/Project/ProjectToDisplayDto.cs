namespace ComplytekTest.API.Application.DTOs.Project
{
    public class ProjectToDisplayDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}

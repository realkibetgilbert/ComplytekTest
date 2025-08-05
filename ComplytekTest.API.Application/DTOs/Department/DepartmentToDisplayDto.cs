namespace ComplytekTest.API.Application.DTOs.Department
{
    public class DepartmentToDisplayDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OfficeLocation { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}

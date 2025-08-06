namespace ComplytekTest.API.Application.DTOs.Employee
{
    public class EmployeeToDisplayDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime CreatedOn { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}

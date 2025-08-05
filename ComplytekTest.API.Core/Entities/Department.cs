namespace ComplytekTest.API.Core.Entities
{
    public class Department
    {
        public long Id { get; set; }

        
        public required string Name { get; set; }

        public required string OfficeLocation { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}

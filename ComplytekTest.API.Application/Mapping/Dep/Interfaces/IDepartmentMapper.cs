using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Dep.Interfaces
{
    public interface IDepartmentMapper
    {
        Department ToDomain(DepartmentToCreateDto departmentToCreateDto);
        DepartmentToDisplayDto ToDisplay(Department department);
    }
}

using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Emp.Interfaces
{
    public interface IEmployeeMapper
    {
        Employee ToDomain(EmployeeToCreateDto employeeToCreateDto);
        EmployeeToDisplayDto ToDisplay(Employee employee);
        List<EmployeeToDisplayDto> ToDisplay(IEnumerable<Employee> employees);
        Employee ToDomain(EmployeeToUpdateDto employeeToUpdateDto);
    }
}

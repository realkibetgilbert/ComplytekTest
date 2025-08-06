using AutoMapper;
using ComplytekTest.API.Application.DTOs.Employee;
using ComplytekTest.API.Application.Mapping.Emp.Interfaces;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Emp.Services
{
    public class EmployeeMapper : IEmployeeMapper
    {
        private readonly IMapper _mapper;

        public EmployeeMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public EmployeeToDisplayDto ToDisplay(Employee employee)
        {
            return _mapper.Map<EmployeeToDisplayDto>(employee);
        }

        public List<EmployeeToDisplayDto> ToDisplay(IEnumerable<Employee> employees)
        {
            return _mapper.Map<List<EmployeeToDisplayDto>>(employees);

        }

        public Employee ToDomain(EmployeeToCreateDto employeeToCreateDto)
        {
            return _mapper.Map<Employee>(employeeToCreateDto);
        }

        public Employee ToDomain(EmployeeToUpdateDto employeeToUpdateDto)
        {
            return _mapper.Map<Employee>(employeeToUpdateDto);
        }
    }
}

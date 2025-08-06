using AutoMapper;
using ComplytekTest.API.Application.DTOs.Department;
using ComplytekTest.API.Application.Mapping.Dep.Interfaces;
using ComplytekTest.API.Core.Entities;

namespace ComplytekTest.API.Application.Mapping.Dep.Services
{
    public class DepartmentMapper : IDepartmentMapper
    {
        private readonly IMapper _mapper;

        public DepartmentMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Department ToDomain(DepartmentToCreateDto departmentToCreateDto)
        {
            return _mapper.Map<Department>(departmentToCreateDto);
        }

        public DepartmentToDisplayDto ToDisplay(Department department)
        {
            return _mapper.Map<DepartmentToDisplayDto>(department);
        }

        public List<DepartmentToDisplayDto> ToDisplay(IEnumerable<Department> departments)
        {
            return _mapper.Map<List<DepartmentToDisplayDto>>(departments);

        }
        public Department ToDomain(DepartmentToUpdateDto departmentToUpdateDto)
        {
            return _mapper.Map<Department>(departmentToUpdateDto);
        }
    }
}

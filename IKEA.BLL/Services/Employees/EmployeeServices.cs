using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Employess;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Employees
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeDto> GetEmployess(string search)
        {
            return _unitOfWork.EmployeeRepository.GetAllAsQuarable().Where(X=>!X.IsDeleted&&(string.IsNullOrEmpty(search)||X.Name.ToLower().Contains(search.ToLower()))).Include(E=>E.Department).Select(employee => new EmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                IsActive = employee.IsActive,
                Salary = employee.Salary,
                Email = employee.Email,
                Gender =employee.Gender.ToString(),
                EmployeeType =employee.EmployeeType.ToString(),
                Department=employee.Department.Name

            });
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
           var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is { })
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType, 
                    Department=employee.Department.Name

                };
            return null;

        }
        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Adress,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifictionBy = 1,
                LastModifictionOn = DateTime.UtcNow,
                
            };
             _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.Complete();
        }

        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Adress,
                IsActive = employeeDto.IsActive,
                Salary =employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId= employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifictionBy = 1,
                LastModifictionOn = DateTime.UtcNow,
            };
             _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.Complete(); 
        }
        public bool DeleteEmployee(int id)
        {
            var employeeRepo =_unitOfWork.EmployeeRepository;
            var emplpoyee = employeeRepo.GetById(id);
            if(emplpoyee is { })
            {
                employeeRepo.Delete(emplpoyee);
                
            }
            return _unitOfWork.Complete() > 0;
        }
    }
}

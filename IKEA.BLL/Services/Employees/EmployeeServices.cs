using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Common.Services;
using IKEA.BLL.Models.Employess;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IKEA.BLL.Services.Employees
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public EmployeeServices(IUnitOfWork unitOfWork,IAttachmentService attachmentService) 
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async Task< IEnumerable<EmployeeDto>> GetEmployessAsync(string search)
        {
            return await _unitOfWork.EmployeeRepository.GetAllAsQuarable().Where(X=>!X.IsDeleted&&(string.IsNullOrEmpty(search)||X.Name.ToLower().Contains(search.ToLower()))).Include(E=>E.Department).Select(employee => new EmployeeDto()
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

            }).ToListAsync();
        }

        public async Task< EmployeeDetailsDto?> GetEmployeeByIdAsync(int id)
        {
           var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
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
                    Department=employee.Department.Name,
                    Image= employee.Image,

                };

            return null;

        }
        public async Task< int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
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
            if (employeeDto.Image is not null)
            {
                employee.Image = _attachmentService.UploadFile(employeeDto.Image, "images");
            }

            _unitOfWork.EmployeeRepository.Add(employee);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task< int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
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
               // Image = employeeDto.Image.ToString() ?? string.Empty,
            };

            if (employeeDto.Image is not null)
            {
                employee.Image = _attachmentService.UploadFile(employeeDto.Image, "images");
            }


            _unitOfWork.EmployeeRepository.Update(employee);
            return await _unitOfWork.CompleteAsync(); 
        }
        public async Task< bool> DeleteEmployeeAsync(int id)
        {
            var employeeRepo =_unitOfWork.EmployeeRepository;
            var emplpoyee = await employeeRepo.GetByIdAsync(id);
            if(emplpoyee is { })
            {
                employeeRepo.Delete(emplpoyee);
                
            }
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task< IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsQuarable().Where(X=>!X.IsDeleted).Select(department => new DepartmentToReturnDTO
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
               
                CreationDate = department.CreationDate,
            }).AsNoTracking().ToListAsync();
            return departments;
        }

        public async  Task< DepartmentDetailsToReturnDTO?> GetDepartmentByIdAsync(int id)
        {
           var department= await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is not null)
            {
                return new DepartmentDetailsToReturnDTO
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifictionBy = department.LastModifictionBy,
                    LastModifictionOn = department.LastModifictionOn
                };
            }
            return null;
            
        }
        public async Task< int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO)
        {
            var createddepatment = new Department
            {
                Code = departmentDTO.Code,
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                CreationDate = departmentDTO.CreationDate,
                CreatedBy = 1,
                LastModifictionBy = 1,
                LastModifictionOn = DateTime.UtcNow,
                //CreatedOn = DateTime.UtcNow
            };
             _unitOfWork.DepartmentRepository.Add(createddepatment);
            return await _unitOfWork.CompleteAsync();
        }
        public async Task< int> UpdateDepartmentAsync(UpdatedDepartmentDTO departmentDTO)
        {
            var updateddepartment = new Department
            {
                Id = departmentDTO.Id,
                Code = departmentDTO.Code,
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                CreationDate = departmentDTO.CreationDate,
                LastModifictionBy =1,
                LastModifictionOn = DateTime.UtcNow,
            };
             _unitOfWork.DepartmentRepository.Update(updateddepartment);
            return await _unitOfWork.CompleteAsync();
        }
         public async Task< bool> DeleteDepartmentAsync(int id)
        {
            var departmentrepo =_unitOfWork.DepartmentRepository;
            var department = await departmentrepo.GetByIdAsync(id);
            if(department is not null)
            {
                 departmentrepo.Delete(department);
            }
            return await _unitOfWork.CompleteAsync()>0;
        }
    }
}

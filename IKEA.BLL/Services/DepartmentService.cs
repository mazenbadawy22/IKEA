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


        public IEnumerable<DepartmentToReturnDTO> GetAllDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAllAsQuarable().Where(X=>!X.IsDeleted).Select(department => new DepartmentToReturnDTO
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
               
                CreationDate = department.CreationDate,
            }).AsNoTracking().ToList();
            return departments;
        }

        public DepartmentDetailsToReturnDTO? GetDepartmentById(int id)
        {
           var department= _unitOfWork.DepartmentRepository.GetById(id);
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
        public int CreateDepartment(CreatedDepartmentDTO departmentDTO)
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
            return _unitOfWork.Complete();
        }
        public int UpdateDepartment(UpdatedDepartmentDTO departmentDTO)
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
            return _unitOfWork.Complete();
        }
         public bool DeleteDepartment(int id)
        {
            var departmentrepo =_unitOfWork.DepartmentRepository;
            var department = departmentrepo.GetById(id);
            if(department is not null)
            {
                 departmentrepo.Delete(department);
            }
            return _unitOfWork.Complete()>0;
        }
    }
}

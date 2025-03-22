using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;

namespace IKEA.BLL.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDetailsToReturnDTO?> GetDepartmentByIdAsync(int id);
        Task<int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO);
        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO departmentDTO);
        Task<bool> DeleteDepartmentAsync(int id);

    }   
}

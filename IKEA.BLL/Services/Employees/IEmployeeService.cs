using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Employess;

namespace IKEA.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        Task <IEnumerable<EmployeeDto>> GetEmployessAsync(string search);
        Task< EmployeeDetailsDto?> GetEmployeeByIdAsync(int id);
        Task< int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto);
        Task< int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto);
        Task< bool> DeleteEmployeeAsync(int id);

    }
}

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
        IEnumerable<EmployeeDto> GetEmployess(string search);
        EmployeeDetailsDto? GetEmployeeById(int id);
        int CreateEmployee (CreatedEmployeeDto employeeDto);
        int UpdateEmployee (UpdatedEmployeeDto employeeDto);
        bool DeleteEmployee (int id);

    }
}

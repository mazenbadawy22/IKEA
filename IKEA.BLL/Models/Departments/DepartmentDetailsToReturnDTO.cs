using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Models.Departments
{
    public class DepartmentDetailsToReturnDTO
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifictionBy { get; set; }
        public DateTime LastModifictionOn { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        [Display(Name ="Creation Data ")]
        public DateOnly CreationDate { get; set; }
    }
}

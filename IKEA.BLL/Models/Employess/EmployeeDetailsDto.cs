﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Common.Enums;

namespace IKEA.BLL.Models.Employess
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? Address { get; set; }
        public decimal? Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name ="Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Display(Name ="Hiring Date")]
        public DateTime HiringDate { get; set; }
        public Gender Gender { get; set; } 
        public EmployeeType EmployeeType { get; set; }
        #region Adminstration
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        #endregion
    }
}

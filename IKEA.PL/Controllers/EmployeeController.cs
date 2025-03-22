using IKEA.BLL.Models.Departments;
using IKEA.BLL.Models.Employess;
using IKEA.BLL.Services;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {

        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        

        public EmployeeController(IEmployeeService employeeService,IWebHostEnvironment webHostEnvironment,ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
           
        }
        #endregion
        #region Index
        [HttpGet]
        public async Task< ActionResult> Index(string search)
        {
            var employees = await _employeeService.GetEmployessAsync(search);
            return View(employees);
        }
        #endregion
        #region Create
        #region get
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           // ViewData["Departments"] = departmentService.GetAllDepartments();
            return View ();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var message = string.Empty;
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Employee Is Not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employee);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_webHostEnvironment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employee);
                }
                else
                {
                    message = "Employee Is Not Created";
                    return View("Error", message);
                }
            }


        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
        public async Task< IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            return View(employee);
        }
        #endregion
        #region Edit
        #region get
        [HttpGet]

        public async Task< IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
           
            // ViewData["Departments"] = departmentService.GetAllDepartments();
            return View(new UpdatedEmployeeDto()
            {
                
                Name = employee.Name,
                Adress = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
            });
        }

        #endregion
        #region Post
        [HttpPost] // POST
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;
            try
            {
                //if (newImage is not null)
                //{
                //    employee.Image = _attachmentService.UploadFile(newImage, "images");
                //}
                var updated = await _employeeService.UpdateEmployeeAsync(employee) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "Employee is not Updated";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Set Message
                _logger.LogError(ex, ex.Message);

                if (_webHostEnvironment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "The Employee is not Created";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion
        #endregion
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Delete(int id)
        {
            var message = string.Empty;
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            try
            {
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry An Ocuuerd During Deleting The Employee";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "Sorry An Ocuuerd During Deleting The Employee";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}

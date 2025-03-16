using IKEA.BLL.Models.Departments;
using IKEA.BLL.Models.Employess;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index()
        {
            var employees = _employeeService.GetAllEmployess();
            return View(employees);
        }
        #endregion
        #region Create
        #region get
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var message = string.Empty;
            try
            {
                var result = _employeeService.CreateEmployee(employee);
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
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = _employeeService.GetEmployeeById(id.Value);
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

        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
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
        public IActionResult Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;
            try
            {
                var updated = _employeeService.UpdateEmployee(employee) > 0;
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
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            var deleted = _employeeService.DeleteEmployee(id);

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

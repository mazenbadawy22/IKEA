using IKEA.BLL.Models.Departments;
using IKEA.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<CreatedDepartmentDTO> _logger;
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService,ILogger<CreatedDepartmentDTO> logger,IWebHostEnvironment environment)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
        }
        #region index
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAllDepartments();
            return View(departments);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        #endregion
        #region post
        [HttpPost]
        public IActionResult Create(CreatedDepartmentDTO department)
        {
            if(!ModelState.IsValid)
            {
                return View(department);
            }
            var message = string.Empty;
            try
            {
                var result = _departmentService.CreateDepartment(department);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Department Is Not Created";
                    ModelState.AddModelError(string.Empty,message );
                    return View(department);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                if(_environment.IsDevelopment())
                {
                    message=ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Department Is Not Created";
                    return View("Error",message);
                }
            }
            
           
        }
        #endregion
        #endregion



    }
}

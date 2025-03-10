using IKEA.BLL.Models.Departments;
using IKEA.BLL.Services;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<CreatedDepartmentDTO> _logger;
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService, ILogger<CreatedDepartmentDTO> logger, IWebHostEnvironment environment)
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
            if (!ModelState.IsValid)
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
                    ModelState.AddModelError(string.Empty, message);
                    return View(department);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Department Is Not Created";
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
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        #endregion
        #region Edit
        #region Get
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(new DepartmentEditViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate
            });
        }
        #endregion
        #region Post
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var updatedDepartment = new UpdatedDepartmentDTO()
                {
                    Id = id, 
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate,


                };
                var updated = _departmentService.UpdateDepartment(updatedDepartment) > 0;
                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry , An Error Ocuured While Updating The Department";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry , An Error Ocuured While Updating The Department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View (departmentVM);
        }
        #endregion
        #endregion
        #region Delete
        #region Get
        [HttpGet]
        public IActionResult Delete(int? id)
        {
         if(id is null)
            {
                return BadRequest();
            }
         var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        #endregion
        #region Post
        [HttpPost]
        public IActionResult Delete (int id)
        {
            var message = string.Empty;
            var deleted = _departmentService.DeleteDepartment(id);
           
            try
            {
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry An Ocuuerd During Deleting The Department";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                message=_environment.IsDevelopment()?ex.Message : "Sorry An Ocuuerd During Deleting The Department";
            }
            return RedirectToAction (nameof(Index));
        }
        #endregion
        #endregion




    }
    }

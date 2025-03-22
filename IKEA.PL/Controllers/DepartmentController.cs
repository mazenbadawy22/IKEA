using AutoMapper;
using IKEA.BLL.Models.Departments;
using IKEA.BLL.Services;
using IKEA.DAL.Models.Departments;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<CreatedDepartmentDTO> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, ILogger<CreatedDepartmentDTO> logger, IWebHostEnvironment environment,IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }
        #region index
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            ViewData["Message"] = "Hello ViewData";
            ViewBag.Message = "Hello ViewBag";
            var departments =  await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
        public async Task< IActionResult> Create()
        {
            return View();
        }
        #endregion
        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(DepartmentEditViewModel departmentVM)
        {

            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var CreatedDepartment = _mapper.Map<CreatedDepartmentDTO>(departmentVM);
                var result = await _departmentService.CreateDepartmentAsync(CreatedDepartment);

                if (result > 0)
                {
                    TempData["Message"] = "Department Is created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Department Has Not Been Created";
                    message = "Department Is Not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
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
        public async Task< IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
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
        public async  Task< IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            //return View(new DepartmentEditViewModel()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    Description = department.Description,
            //    CreationDate = department.CreationDate
            //});
            var DepartmentViewModel = _mapper.Map<DepartmentDetailsToReturnDTO, DepartmentEditViewModel>(department);
            return View(DepartmentViewModel);
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                //var updatedDepartment = new UpdatedDepartmentDTO()
                //{
                //    Id = id, 
                //    Code = departmentVM.Code,
                //    Name = departmentVM.Name,
                //    Description = departmentVM.Description,
                //    CreationDate = departmentVM.CreationDate,


                //};
                var UpdatedDepartment = _mapper.Map<UpdatedDepartmentDTO>(departmentVM);
                var Result = await _departmentService.UpdateDepartmentAsync(UpdatedDepartment) > 0;
                if (Result)
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
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id is null)
        //    {
        //        return BadRequest();
        //    }
        //    var department = _departmentService.GetDepartmentById(id.Value);
        //    if (department is null)
        //    {
        //        return NotFound();
        //    }
        //    return View(department);
        //}
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Delete(int id)
        {
            var message = string.Empty;


            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry An Ocuuerd During Deleting The Department";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry An Ocuuerd During Deleting The Department";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
        #endregion




    }
}

using IKEA.DAL.Models.Identity;
using IKEA.PL.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        #region Get
        [HttpGet]
        public async Task< IActionResult> Singup()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task< IActionResult> Singup(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var ExistingUser = await  _userManager.FindByNameAsync(signUpViewModel.UserName);
            if(ExistingUser != null)
            {
                ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This UserName Is Already Taken");
                return View(signUpViewModel);
            }
            var User = new ApplicationUser()
            {
                FName=signUpViewModel.FirstName,
                LName=signUpViewModel.LastName,
                UserName = signUpViewModel.UserName,
                Email=signUpViewModel.Email,
                IsAgree=signUpViewModel.IsAgree,

            };
            var Result = await _userManager.CreateAsync(User,signUpViewModel.Password);
            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }
            foreach(var error in Result.Errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);
            }
            return View(signUpViewModel);
        }
        #endregion
        #endregion
        #region Login
        #region Get
        [HttpGet]
        public async Task< IActionResult> SignIn()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninViewModel signinViewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var User = await _userManager.FindByEmailAsync(signinViewModel.Email);
            if (User is { })
            {
                var flag = await _userManager.CheckPasswordAsync(User,signinViewModel.Password);
                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(User, signinViewModel.Password, signinViewModel.RememberMe,true);
                    if(result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet");
                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account Is locked");
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                   
                }
               
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(signinViewModel);

        }
        #endregion
        #endregion
        #region Logout
        
        public async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion
        #region Forget Password

        #endregion
        #region Send Email

        #endregion
        #region Reset Password

        #endregion



    }
}

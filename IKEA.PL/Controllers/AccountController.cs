using IKEA.BLL.Common.Services.EmailSettings;
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
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }
        #region Register
        #region Get
        [HttpGet]
        public async Task< IActionResult> Signup()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task< IActionResult> Signup(SignUpViewModel signUpViewModel)
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
        #region Get
        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (User is { })
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var url = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email,token=token  },Request.Scheme);
                    // To , Subject , Body
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url
                    };
                    _emailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
                ModelState.AddModelError(string.Empty, "InValid Operation, Pls Try Again");
            }
            return View(forgetPasswordViewModel);
        }
        #endregion
        #endregion
        #region CheckYourInbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion
        #region Reset Password
        #region Get
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if(user is { })
                {
                  var result= await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                }

            }
            ModelState.AddModelError(string.Empty,"Unable to reset your password.");
            return View(resetPasswordViewModel);
        }
        #endregion
        #endregion



    }
}

using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Register
        [HttpGet]
        public IActionResult Singup()
        {
            return View();
        }
        #endregion
        #region Login
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }
        #endregion
        #region Logout
        [HttpGet]
        public IActionResult SignOut()
        {
            return View();
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

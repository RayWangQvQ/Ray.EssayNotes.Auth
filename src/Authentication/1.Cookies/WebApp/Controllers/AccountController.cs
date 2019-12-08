using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//添加引用：
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 异步登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Normally Identity handles sign in, but you can do it directly
            if (ValidateLogin(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", "Member")
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }

            return View();
        }


        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateLogin(string userName, string password)
        {
            // 用于测试，任何用户名和密码都验证通过
            return true;
        }
    }
}
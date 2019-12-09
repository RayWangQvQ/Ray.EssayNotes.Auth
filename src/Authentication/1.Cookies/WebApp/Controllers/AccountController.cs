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
        /// 访问登录页面
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
        /// 异步登录并跳转
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // 通常是Identity处理登录验证, 但是我们也可以自己处理
            if (!ValidateLogin(userName, password)) return View();

            //生成Claim断言集合
            var claims = new List<Claim>
            {
                new Claim("user", userName),
                new Claim("role", "Member")
            };
            //生成Identity身份
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies", "user", "role");
            //生成Principal持有人
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }

        /// <summary>
        /// 拒绝访问跳转
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
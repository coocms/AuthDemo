using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthWithCookie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录接口  并将查询到的身份信息写入cookies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(string name, string password)
        {

            var claims = new List<Claim>()//鉴别你是谁，相关信息
                    {
                        new Claim(ClaimTypes.Role,"Admin"),
                        new Claim(ClaimTypes.Role,"User"),
                        new Claim(ClaimTypes.Name,name),
                        new Claim("password",password),//可以写入任意数据
                        new Claim("Account","Administrator"),
                        new Claim("role","admin"),
                        new Claim("zhaoxi","zhaoxi"),
                        new Claim("User","zhaoxi")
                    };
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),//过期时间：30分钟

            }).Wait();
            var user = HttpContext.User;


            return Ok();
        }
    }
}

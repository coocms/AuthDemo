using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace jwtServerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        JwtHelper _jwtHelper;
        public UserController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }
        [HttpPost("Login")]
        public IActionResult Login(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Password))
            {
                return BadRequest();
            }
            //验证逻辑
            var token = _jwtHelper.CreateToken(userInfo.UserName);

            #region 写cookie 到浏览器
            Response.Cookies.Append("MyCookie", "Hello, World!", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true, // 设置为 true 表示 JavaScript 不能访问该 cookie

                SameSite = SameSiteMode.None,//跨域设置
                Secure = true //跨域设置
            });
            #endregion
            return Ok(token);
        }

        /// <summary>
        /// 获取自己的详细信息，其中 [Authorize] 就表示要带Token才行
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetSelfInfo")]
        [Authorize]
        public IActionResult GetSelfInfo()
        {
            //执行到这里，就表示已经验证授权通过了
            /*
             * 这里返回个人信息有两种方式
             * 第一种：从Header中的Token信息反向解析出用户账号，再从数据库中查找返回
             * 第二种：从Header中的Token信息反向解析出用户账号信息直接返回，当然，在前面创建        Token时，要保存进使用到的Claims中。
            */
            return Ok("授权通过了！");
        }
    }
}

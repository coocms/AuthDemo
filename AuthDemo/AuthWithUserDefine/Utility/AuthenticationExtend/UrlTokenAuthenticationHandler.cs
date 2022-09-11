using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthWithUserDefine.Utility.AuthenticationExtend
{
    /// <summary>
    /// 通过URL参数做鉴权--
    /// 需要在Startup注册映射
    /// </summary>
    public class UrlTokenAuthenticationHandler : IAuthenticationHandler //, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        /// <summary>
        /// 核心鉴权处理方法
        /// 解析用户信息
        /// </summary>
        /// <returns></returns>
        public Task<AuthenticateResult> AuthenticateAsync()
        {
            Console.WriteLine($"This is {nameof(UrlTokenAuthenticationHandler)}.AuthenticateAsync");
            string userInfo = _HttpContext.Request.Query["UrlToken"];//信息从哪里读
            Console.WriteLine($"获取token：{userInfo}");
            if (string.IsNullOrWhiteSpace(userInfo))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            else if ("Coocms".Equals(userInfo))//信息是否可靠？  校验规则可以传递到Option的
            {
                var claimIdentity = new ClaimsIdentity("Custom");
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, userInfo));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Email, "18672713698@163.com"));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Country, "China"));
                claimIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth, "1986"));
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimIdentity);//信息拼装和传递

                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, null, _AuthenticationScheme.Name)));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail($"UrlToken is wrong: {userInfo}"));
            }

        }

        /// <summary>
        /// 如果解析找不到用户身份信息，就来执行这个方法
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"This is {nameof(UrlTokenAuthenticationHandler)}.ChallengeAsync");
            //可以将请求重定向到指定路径
            //string redirectUri = "/Ninth/SetUrlToken";
            //_HttpContext.Response.Redirect(redirectUri);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 未授权，无权限---授权方面：有用户信息--但是根据用户信息计算后，还是不能访问资源
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"This is {nameof(UrlTokenAuthenticationHandler)}.ForbidAsync");
            _HttpContext.Response.StatusCode = 403;
            return Task.CompletedTask;
        }


        /// <summary>
        /// 授权方案--渠道---通过Url地址中的参数来解析判断
        /// </summary>
        private AuthenticationScheme _AuthenticationScheme = null;//"UrltokenScheme"
        private HttpContext _HttpContext = null;

        /// <summary>
        /// 初始化，Provider传递进来的，方法注入
        /// 像方法注入
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Console.WriteLine($"This is {nameof(UrlTokenAuthenticationHandler)}.InitializeAsync");
            _AuthenticationScheme = scheme;
            _HttpContext = context;
            return Task.CompletedTask;
        }


        ///// <summary>
        ///// SignInAsync和SignOutAsync使用了独立的定义接口，
        ///// 因为在现代架构中，通常会提供一个统一的认证中心，负责证书的颁发及销毁（登入和登出），
        ///// 而其它服务只用来验证证书，并用不到SingIn/SingOut。
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="properties"></param>
        ///// <returns></returns>
        //public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        //{
        //    var ticket = new AuthenticationTicket(user, properties, this._AuthenticationScheme.Name);
        //    this._HttpContext.Response.Cookies.Append("UrlTokenCookie", Newtonsoft.Json.JsonConvert.SerializeObject(ticket.Principal.Claims)); 
        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// 退出
        ///// </summary>
        ///// <param name="properties"></param>
        ///// <returns></returns>
        //public Task SignOutAsync(AuthenticationProperties properties)
        //{
        //    this._HttpContext.Response.Cookies.Delete("UrlTokenCookie");
        //    return Task.CompletedTask;
        //} 
    }

    /// <summary>
    /// 提供个固定值
    /// </summary>
    public class UrlTokenAuthenticationDefaults
    {
        /// <summary>
        /// 提供固定名称
        /// </summary>
        public const string AuthenticationScheme = "UrlTokenScheme";
    }
}

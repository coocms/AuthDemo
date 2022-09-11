using AuthWithUserDefine.Utility.AuthenticationExtend;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthWithUserDefine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            AuthenticateResult? UrlTokenResult = base.HttpContext.AuthenticateAsync(UrlTokenAuthenticationDefaults.AuthenticationScheme).Result;
            string UrlTokenUserInfo = string.Empty;
            if (UrlTokenResult.Succeeded)
            {
                UrlTokenUserInfo = Newtonsoft.Json.JsonConvert.SerializeObject(UrlTokenResult?.Principal?.Claims.Select(u => new
                {
                    u.Type,
                    u.Value
                }));
            }

            return Ok("访问成功");
        }

    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]//如果只是标记了Authorize 只要解析到了用户信息就验证通过， 如果没有鉴权授权失败  就会跳转到指定的loginPath
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            AuthenticateResult? cookiesResult =  base.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            string CookiesInfo = string.Empty;
            if (cookiesResult.Succeeded)
            {
                //通过Cookies 获取到 claims
                CookiesInfo = Newtonsoft.Json.JsonConvert.SerializeObject(cookiesResult?.Principal?.Claims.Select(u => new
                {
                    u.Type,
                    u.Value
                }));
                return Ok(CookiesInfo);
            }
            else
            {
                CookiesInfo = "没有用户身份信息";
            }


            return BadRequest();
        }
    }
}
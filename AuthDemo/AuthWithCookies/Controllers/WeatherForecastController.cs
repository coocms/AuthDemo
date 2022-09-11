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

        [Authorize(Roles = "Admin")]//���ֻ�Ǳ����Authorize ֻҪ���������û���Ϣ����֤ͨ���� ���û�м�Ȩ��Ȩʧ��  �ͻ���ת��ָ����loginPath
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            AuthenticateResult? cookiesResult =  base.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            string CookiesInfo = string.Empty;
            if (cookiesResult.Succeeded)
            {
                //ͨ��Cookies ��ȡ�� claims
                CookiesInfo = Newtonsoft.Json.JsonConvert.SerializeObject(cookiesResult?.Principal?.Claims.Select(u => new
                {
                    u.Type,
                    u.Value
                }));
                return Ok(CookiesInfo);
            }
            else
            {
                CookiesInfo = "û���û������Ϣ";
            }


            return BadRequest();
        }
    }
}
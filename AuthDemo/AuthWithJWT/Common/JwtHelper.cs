using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthWithJWT.Common
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// 创建Token 这里面可以保存自己想要的信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string CreateToken(string username, string mobile)
        {
            // 1. 定义需要使用到的Claims
            var claims = new[]
            {
                new Claim("username", username),
                new Claim("mobile", mobile),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"User"),
                /* 可以保存自己想要信息，传参进来即可
                new Claim("sex", "sex"),
                new Claim("limit", "limit"),
                new Claim("head_url", "xxxxx")
                */
            };

            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],    //Issuer
                _configuration["Jwt:Audience"],  //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddMinutes(30),     //expires
                signingCredentials               //Credentials
            );

            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}

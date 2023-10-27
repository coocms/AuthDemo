﻿using AuthWithJWT.Common;
using AuthWithJWT.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;

namespace AuthWithJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private JwtHelper _jwtHelper;
        public UserController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }


        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetToken(UserInfo user)
        {
            //参数验证等等....
            if (string.IsNullOrEmpty(user.UserName))
            {
                return Ok("参数异常！");
            }

            //这里可以连接mysql数据库做账号密码验证


            //这里可以做Redis缓存验证等等


            //这里获取Token，当然，这里也可以选择传结构体过去
            var token = _jwtHelper.CreateToken(user.UserName, user.PhoneNumber);
            return Ok(token);
        }


        /// <summary>
        /// 获取自己的详细信息，其中 [Authorize] 就表示要带Token才行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult GetSelfInfo()
        {
            //执行到这里，就表示已经验证授权通过了
            /*
             * 这里返回个人信息有两种方式
             * 第一种：从Header中的Token信息反向解析出用户账号，再从数据库中查找返回
             * 第二种：从Header中的Token信息反向解析出用户账号信息直接返回，当然，在前面创建        Token时，要保存进使用到的Claims中。
            */
            AuthenticateResult? jwtResult = base.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).Result;
            string JwtInfo = string.Empty;
            if (jwtResult.Succeeded)
            {
                //通过Cookies 获取到 claims
                JwtInfo = Newtonsoft.Json.JsonConvert.SerializeObject(jwtResult?.Principal?.Claims.Select(u => new
                {
                    u.Type,
                    u.Value
                }));
                return Ok(JwtInfo);
            }
            else
            {
                JwtInfo = "没有用户身份信息";
            }

            return Ok("授权通过了！");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminTest()
        {
            return Ok("授权通过了！");
        }
    }
}

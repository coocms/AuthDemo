
using Microsoft.AspNetCore.Authentication.Cookies;

namespace jwtServerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<JwtHelper>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    #region 允许所有（但是允许所有后不能再允许cookie 传递）
                    //builder.AllowAnyOrigin()
                    //        .AllowAnyMethod()
                    //        .AllowAnyHeader();
                    #endregion
                    builder.WithOrigins("http://192.168.50.87:5021", "http://localhost:5021") //允许前端跨域访问的地址列表
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
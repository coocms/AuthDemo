using AuthWithUserDefine.Utility.AuthenticationExtend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//配置鉴权策略
builder.Services.AddAuthentication(options =>
{
    options.AddScheme<UrlTokenAuthenticationHandler>("UrlTokenScheme", "UrlTokenSchemeDemo");
    options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;  //指定默认的鉴权渠道//不能少
    options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

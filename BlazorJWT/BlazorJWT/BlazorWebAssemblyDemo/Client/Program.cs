using Blazored.LocalStorage;
using BlazorWebAssemblyDemo.Client;
using BlazorWebAssemblyDemo.Client.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlazorWebAssemblyDemo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration["LorawanWebApiAddr"]),
                //BaseAddress = new Uri("http://192.168.50.87:9009"),
                Timeout = TimeSpan.FromSeconds(3)
            });
            
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
            builder.Services.AddBlazoredLocalStorageAsSingleton();
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None; // ªÚ’ﬂ SameSiteMode.Strict ªÚ SameSiteMode.Lax

            });

            var app = builder.Build();

            await app.RunAsync();
        }
    }
}
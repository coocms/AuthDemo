using Blazored.LocalStorage;
using BlazorWebAssemblyDemo.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BlazorWebAssemblyDemo.Client.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IJSRuntime _jsruntime;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage, IJSRuntime jSRuntime)
        {
            _jsruntime = jSRuntime;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginRqtDto rqtDto)
        {

            HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                userName = rqtDto.Account,
                password = rqtDto.Password
            }), Encoding.UTF8, "application/json");

            // 设置默认请求头
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // 构造 POST 请求
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/User/Login");
            request.Content = content;
            // 设置 credentials 为 'include'  需要加此配置，允许cookie跨域传递
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            // 发送请求
            var rsp = await _httpClient.SendAsync(request);

            //var rsp = await _httpClient.PostAsync("/api/User/Login", content);

            if (!rsp.IsSuccessStatusCode)
            {
                return false;
            }
            var authToken = await rsp.Content.ReadAsStringAsync();

            await _localStorage.SetItemAsync("authToken", authToken);
            ((AuthProvider)_authenticationStateProvider).MarkUserAsAuthenticated(rqtDto.Account);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            
            //await _jsruntime.InvokeVoidAsync("eval", $"document.cookie = 'MyCookie=Hello, World!; expires=Thu, 01 Jan 2025 00:00:00 UTC; path=/;';");//使用js 设置cookie   
            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> GetSelfInfo()
        {
            var rsp = await _httpClient.PostAsync("/api/User/GetSelfInfo", null);

            if (!rsp.IsSuccessStatusCode)
            {
                return "授权失败";
            }
            return await rsp.Content.ReadAsStringAsync();
        }
    }
}

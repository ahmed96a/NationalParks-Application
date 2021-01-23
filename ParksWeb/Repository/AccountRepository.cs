using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParksModels.Dtos;
using ParksWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParksWeb.Repository
{
    // 13. Part 1
    // ---------------------------

    public class AccountRepository : IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 13. Part 2
        // -----------------------

        public async Task<UserDto> LoginAsync(string url, LoginDto loginDto, string token)
        {
            if (loginDto != null)
            {
                var httpClient = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

                // 13. Part 6
                // ----------------------
                if (token != null && token.Length > 0)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                // ----------------------

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserDto>(jsonContent);
                }
            }
            return null;
        }

        public async Task<bool> RegisterAsync(string url, RegisterDto registerDto, string token)
        {
            if (registerDto != null)
            {
                var httpClient = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

                // 13. Part 6
                // ----------------------
                if (token != null && token.Length > 0)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                // ----------------------

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }

        // -----------------------
    }

    // ---------------------------
}

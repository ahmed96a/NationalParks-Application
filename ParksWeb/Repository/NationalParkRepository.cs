using Newtonsoft.Json;
using ParksModels.Dtos;
using ParksModels.Models;
using ParksWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParksWeb.Repository
{
    // 8. Part 8
    // ---------------------------------

    public class NationalParkRepository: INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NationalParkRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(string Url, NationalParkCreateDto objToCreate, string token)
        {
            if(objToCreate != null)
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, Url);
                httpRequest.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");

                HttpClient httpClient = _httpClientFactory.CreateClient();

                // 13. Part 6
                // ----------------------
                if (token != null && token.Length > 0)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                // ----------------------

                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string Url, int Id, string token)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete, Url + Id);
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // 13. Part 6
            // ----------------------
            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // ----------------------

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<NationalParkDto>> GetAllSync(string Url, string token)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, Url);
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // 13. Part 6
            // ----------------------
            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // ----------------------

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
            
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                IEnumerable<NationalParkDto> nationalParksDto = JsonConvert.DeserializeObject<IEnumerable<NationalParkDto>>(jsonString);

                return nationalParksDto;
            }

            return null;
        }

        public async Task<NationalParkDto> GetAsync(string Url, int Id, string token)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, Url + Id);
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // 13. Part 6
            // ----------------------
            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // ----------------------

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                NationalParkDto nationalParkDto = JsonConvert.DeserializeObject<NationalParkDto>(jsonString);

                return nationalParkDto;
            }

            return null;
        }

        public async Task<bool> UpdateAsync(string Url, int nationalParkId, NationalParkUpdateDto objToUpdate, string token)
        {
            if (objToUpdate != null)
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Put, Url + nationalParkId);
                httpRequest.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");

                HttpClient httpClient = _httpClientFactory.CreateClient();

                // 13. Part 6
                // ----------------------
                if (token != null && token.Length > 0)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                // ----------------------

                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // ---------------------------------
}

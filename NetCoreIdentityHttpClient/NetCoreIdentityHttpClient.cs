using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreDataAccess.BaseRequests;
using NetCoreDataAccess.BaseResponses;
using NetCoreDomain;
using NetCoreHttpClient;
using NetCoreIdentityHttpClient.Configurations;
using NetCoreIdentityHttpClient.Dtos;
using NetCoreIdentityHttpClient.Requests;
using Newtonsoft.Json;

namespace NetCoreIdentityHttpClient
{
    public class NetCoreIdentityHttpClient : INetCoreIdentityHttpClient
    {
        private readonly NetCoreBaseHttpClient _httpClient;

        public NetCoreIdentityHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            var configuration = new NetCoreIdentityHttpClientConfiguration();
            configuration.Uri = "https://localhost:44312/";
            _httpClient = new NetCoreBaseHttpClient(httpContextAccessor);
            _httpClient.ConfigureServiceHttpClient(new NetCoreHttpClientConfigurationOptions { HttpClientBaseAddress = configuration.Uri });
        }

        public async ValueTask<Result<PagedListResponse<UserDto>>> GetUsersPagedList(GetUsersPagedListRequest usersPagedListRequest)
        {
            return await ProcessPostRequest<Result<PagedListResponse<UserDto>>, GetUsersPagedListRequest>("api/users/getUsersPagedList", usersPagedListRequest);
        }

        public async ValueTask<Result<UserDto>> GetUserById(Guid userId)
        {
            return await ProcessPostRequest<Result<UserDto>, Guid>("api/users/getById", userId);
        }

        public async ValueTask<Result<Guid>> CreateUser(UserDto userDto)
        {
            return await ProcessPostRequest<Result<Guid>, UserDto>("api/users/create", userDto);
        }

        public async ValueTask<Result<bool>> UpdateUser(UserDto userDto)
        {
            return await ProcessPostRequest<Result<bool>, UserDto>("api/users/update", userDto);
        }

        public async ValueTask<Result<bool>> DeleteUser(Guid userId)
        {
            return await ProcessPostRequest<Result<bool>, Guid>("api/users/delete", userId);
        }

        private async ValueTask<TResponse> ProcessGetRequest<TResponse, TRequest>(string apiUri, TRequest request)
        {
            return await ProcessRequest<TResponse, TRequest>(request, (httpClient, byteContent) => httpClient.GetAsync(apiUri).Result);
        }

        private async ValueTask<TResponse> ProcessPostRequest<TResponse, TRequest>(string apiUri, TRequest request)
        {
            return await ProcessRequest<TResponse, TRequest>(request, (httpClient, byteContent) => httpClient.PostAsync(apiUri, byteContent).Result);
        }

        private async ValueTask<TResponse> ProcessRequest<TResponse, TRequest>(TRequest request, Func<HttpClient, ByteArrayContent, HttpResponseMessage> func)
        {
            var httpClient = await _httpClient.GetHttpClient();

            var jsonRequest = JsonConvert.SerializeObject(request);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //var httpClientresponse = httpClient.PostAsync(apiUri, byteContent).Result;
            var httpClientresponse = func(httpClient, byteContent);
            if (!httpClientresponse.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            var jsonResponse = await httpClientresponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            return response;
        }

        public async Task<PagedListResponse<UserDto>> GetAllUsers()
        {
            var client = await _httpClient.GetHttpClient();
            var request = new GetUsersPagedListRequest
            {
                Skip = 0,
                Take = 10,
                Sortings = new List<SortedListRequest>
                {
                    new SortedListRequest
                    {
                        FieldName = "FirstName",
                        Direction = SortDirection.Descending
                    },
                    //new SortedListRequest
                    //{
                    //    FieldName = "Users.MiddleName",
                    //    Direction = SortDirection.Descending
                    //},
                    //new SortedListRequest
                    //{
                    //    FieldName = "Users.SeconfName",
                    //    Direction = SortDirection.Ascending
                    //}
                }
            };
            var jsonRequest = JsonConvert.SerializeObject(request);

            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync("api/users/getUsersPagedList", byteContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var identityUsers = JsonConvert.DeserializeObject<PagedListResponse<UserDto>>(jsonResponse);
            return identityUsers;
        }
    }
}

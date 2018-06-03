using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public NetCoreIdentityHttpClient(IHttpContextAccessor httpContextAccessor, IConfigurationRoot configurationRoot)
        {
            var configuration = new NetCoreIdentityHttpClientConfiguration();
            configurationRoot.GetSection(nameof(NetCoreIdentityHttpClientConfiguration)).Bind(configuration);
            _httpClient = new NetCoreBaseHttpClient(httpContextAccessor);
            _httpClient.ConfigureServiceHttpClient(new NetCoreHttpClientConfigurationOptions { HttpClientBaseAddress = configuration.Uri });
        }

        public async ValueTask<Result<PagedListResponse<UserDto>>> GetUsersPagedList(GetUsersPagedListRequest usersPagedListRequest)
        {
            return await ProcessPostRequest<Result<PagedListResponse<UserDto>>, GetUsersPagedListRequest>("api/users/getUsersPagedList", usersPagedListRequest);
        }

        public async ValueTask<Result<List<UserDto>>> GetUsersByRole(string roleName)
        {
            return await ProcessPostRequest<Result<List<UserDto>>, string>("api/users/getByRole", roleName);
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

        public async ValueTask<Result<List<RoleDto>>> GetRoles()
        {
            return await ProcessPostRequest<Result<List<RoleDto>>>("api/roles/getRoles");
        }

        private async ValueTask<TResponse> ProcessGetRequest<TResponse, TRequest>(string apiUri, TRequest request)
        {
            return await ProcessRequest<TResponse, TRequest>(request, (httpClient, byteContent) => httpClient.GetAsync(apiUri).Result);
        }

        private async ValueTask<TResponse> ProcessPostRequest<TResponse>(string apiUri)
        {
            return await ProcessRequest<TResponse>((httpClient) => httpClient.PostAsync(apiUri, new ByteArrayContent(new byte[0])).Result);
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

            var httpClientresponse = func(httpClient, byteContent);
            if (!httpClientresponse.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            var jsonResponse = await httpClientresponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            return response;
        }

        private async ValueTask<TResponse> ProcessRequest<TResponse>(Func<HttpClient, HttpResponseMessage> func)
        {
            var httpClient = await _httpClient.GetHttpClient();

            var httpClientresponse = func(httpClient);
            if (!httpClientresponse.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }

            var jsonResponse = await httpClientresponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            return response;
        }
    }
}

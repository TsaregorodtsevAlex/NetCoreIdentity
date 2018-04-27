using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreDataAccess.BaseRequests;
using NetCoreDataAccess.BaseResponses;
using NetCoreHttpClient;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;
using NetCoreIdentityHttpClient.Configurations;
using NetCoreIdentityHttpClient.Contracts;
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

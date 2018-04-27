﻿using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreDataAccess.BaseResponses;
using NetCoreDataAccess.Externsions;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUsersPagedListQuery : BaseQuery
    {
        public PagedListResponse<UserDto> Execute(GetUsersPagedListRequest request)
        {
            var response = new PagedListResponse<UserDto>();

            var userRepository = Uow.GetRepository<User>();
            var usersQuery = userRepository.AsQueryable();

            usersQuery = ProcessRequest(request, usersQuery);

            response.Items = usersQuery
                .ApplyPagedListRequest(request, response)
                .Select(UserDto.Map)
                .ToArray();

            return response;
        }

        private static IQueryable<User> ProcessRequest(GetUsersPagedListRequest request, IQueryable<User> usersQuery)
        {
            if (request.HasRequestFirstName)
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(request.FirstName));
            }

            if (request.HasRequestMiddleName)
            {
                usersQuery = usersQuery.Where(u => u.MiddleName.Contains(request.MiddleName));
            }

            if (request.HasRequestSecondName)
            {
                usersQuery = usersQuery.Where(u => u.SecondName.Contains(request.SecondName));
            }

            if (request.HasRequestPosition)
            {
                usersQuery = usersQuery.Where(u => u.Position.Contains(request.Position));
            }

            if (request.HasRequestInn)
            {
                usersQuery = usersQuery.Where(u => u.Inn.Contains(request.Inn));
            }

            if (request.HasRequestBirthdate)
            {
                usersQuery = usersQuery.Where(u => u.Birthdate == request.Birthdate);
            }

            if (request.HasRequestAddress)
            {
                usersQuery = usersQuery.Where(u => u.Address.Contains(request.Address));
            }

            if (request.HasRequestIsActive)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == request.IsActive);
            }

            return usersQuery;
        }
    }
}
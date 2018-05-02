using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Queries;
using NetCoreDataAccess.BaseResponses;
using NetCoreDataAccess.Externsions;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUsersPagedListQuery : BaseQuery
    {
        public Result<PagedListResponse<UserDto>> Execute(GetUsersPagedListRequest request)
        {
            try
            {
                var response = new PagedListResponse<UserDto>();

                var userRepository = Uow.GetRepository<User>();
                var usersQuery = userRepository
                    .AsQueryable()
                    .Where(u => u.IsDeleted == false);

                usersQuery = ProcessRequest(request, usersQuery);

                response.Items = usersQuery
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ApplyPagedListRequest(request, response)
                    .ToArray()
                    .Select(UserDto.MapFromUser)
                    .ToArray();

                return Result<PagedListResponse<UserDto>>.Ok(response);
            }
            catch (Exception exception)
            {
                return Result<PagedListResponse<UserDto>>.Fail(null, exception.Message);
            }
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
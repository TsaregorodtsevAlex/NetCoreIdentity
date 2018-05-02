using System;
using NetCoreCQRS.Commands;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Roles
{
    public class CreateRoleCommand : BaseCommand
    {
        public Result<Guid> Execute(RoleDto roleDto)
        {
            try
            {
                var rolesRepository = Uow.GetRepository<Role>();
                var role = roleDto.ToRole();
                rolesRepository.Create(role);
                Uow.SaveChanges();
                return Result<Guid>.Ok(role.Id);
            }
            catch (Exception exception)
            {
                return Result<Guid>.Fail(Guid.Empty, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}
﻿using System;
using NetCoreCQRS;
using NetCoreCQRS.Queries;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUserByIdQuery: BaseQuery
    {
        public User Execute(Guid userId)
        {
            var userRepository = Uow.GetRepository<User>();
            var user = userRepository.GetById(userId);

            if (user == null)
            {
                //todo throw error
            }

            return user;
        }
    }
}

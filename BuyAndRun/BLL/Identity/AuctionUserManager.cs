﻿using System;
using DAL.Entities.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BLL.Identity
{
    public class AuctionUserManager : UserManager<User>
    {
        public AuctionUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
                                IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
                                IEnumerable<IPasswordValidator<User>> passwordValidators,
                                ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                IServiceProvider services, ILogger<UserManager<User>> logger) : base(store,
                                                                                                     optionsAccessor,
                                                                                                     passwordHasher,
                                                                                                     userValidators,
                                                                                                     passwordValidators,
                                                                                                     keyNormalizer,
                                                                                                     errors, services,
                                                                                                     logger)
        {

        }
    }
}

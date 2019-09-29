﻿using DatingApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp1.Data
{
  public  interface IAuthRepository
    {

        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);

        Task<bool> UserExist(string userName);

    }
}

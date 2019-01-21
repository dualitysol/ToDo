using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    interface IUserService
    {
        Response Register(UserClass user);
    }
}

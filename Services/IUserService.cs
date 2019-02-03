using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IUserService
    {
        List<UserClass> findAll();
        UserClass find(string id);
        UserClass findByEmail(string email);
        void create(UserClass user);
        void update(UserClass user);
        void delete(UserClass user);
        UserClass CreateTask(string userId, TaskClass task);
        UserClass ChangeTaskState(string userId, TaskClass task);
        UserClass DeleteTaskById(string userId, string taskId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public class Response
    {
        public int status { get; set; }
        public string message { get; set; }
        public string error { get; set; }
    }

    public class UserService : IUserService
    {
        private UserClass _user;
        private UserModel userModel = new UserModel();

        public void ToDo ()
        {
            _user = new UserClass();
        }

        public Response Register(UserClass user)
        {  
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                userModel.create(user);
                var response =  new Response();
                response.status = 201;
                response.message = "User created!";
                return response;
            }
            catch
            {
                var response = new Response();
                response.status = 500;
                response.message = "Something went wrong!";
                response.error = "Exception of creating user";
                return response;
            }
        }

        //public Login(UserClass body)
        //{
        //    try
        //    {
        //        var user = userModel.findByEmail(body.Email);
        //        if (user == null)
        //        {
        //            throw MissingMemberException
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        //public

    }
}

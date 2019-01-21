using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ToDo.Models;
using ToDo.Services;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ToDo.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private UserModel userModel = new UserModel();
        private readonly IUserService _userService;
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<UserClass>> Get() => userModel.findAll();

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<UserClass> Get(string id) => userModel.find(id);

        // POST api/values
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SighUp([FromBody] UserClass user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                BadRequest();
            }

            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                userModel.create(user);
                var response = new JsonResult(user);
                response.StatusCode = 201;
                return response;
            }
            catch (Exception err)
            {
                var response = new JsonResult(err);
                response.StatusCode = 400;
                return response;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Produces("application/json")]
        public JsonResult SighIn([FromBody] UserClass body)
        {
            try
            {
                UserClass user = userModel.findByEmail(body.Email);

                var verifyPass = BCrypt.Net.BCrypt.Verify(body.Password, user.Password);
                if (!verifyPass) throw new Exception("Password or email is incorrect!");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSetting["jwtSecret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                // remove password before returning
                user.Password = null;

                var response = new JsonResult(user);
                response.StatusCode = 200;
                return response;
            }
            catch (Exception error)
            {
                var response = new JsonResult(error.Message);
                Console.WriteLine(error);
                response.StatusCode = 404;
                return response;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserClass body)
        {
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

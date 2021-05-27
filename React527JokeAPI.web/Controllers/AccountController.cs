using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using React527JokeAPI.data;

namespace React527JokeAPI.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private string _conn;

        public AccountController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("signup")]
        public void Signup(User u)
        {
            var db = new UserJokeRepository(_conn);
            db.AddUser(u);

        }

        [HttpPost]
        [Route("login")]
        public User Login(User u)
        {
            var db = new UserJokeRepository(_conn);
            var user = db.Login(u);

            if (user is null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim("user", u.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return user;
        }

        [HttpGet]
        [Route("getuser")]
        public User GetCurrentUser()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var db = new UserJokeRepository(_conn);
            return db.GetUserByEmail(User.Identity.Name);
        }

        [HttpPost]
        [Route("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync().Wait();

        }
    }
}


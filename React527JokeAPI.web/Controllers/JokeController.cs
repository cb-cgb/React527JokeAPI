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
using React527JokeAPI.Api;


namespace React527JokeAPI.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private string _conn;
        public JokeController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        private User GetCurrentUser()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var db = new UserJokeRepository(_conn);
            return db.GetUserByEmail(User.Identity.Name);
        }

        [Route("getjoke")]
        [HttpGet]
        public Joke GetJoke()
        {
            var joke = JokeAPI.GetJoke();
            var db = new UserJokeRepository(_conn);
           
            if (!db.JokeExists(joke.ResultJokeId))
            {
                db.AddJoke(joke);
            }

            return db.GetJokeFromDB(joke.ResultJokeId);

            //return joke;
        }

        [Route("likejoke")]
        [HttpPost]
        public void LikeJoke(UserJokeLike uj)
        {
            var user = GetCurrentUser();

            if (user != null)
            {
                var db = new UserJokeRepository(_conn);
                db.AddUpdateJokeLike(uj.JokeId, user.Id, uj.Like);
            }
        }

        //raw sql approach
        //[Route("getall")]
        //[HttpGet]
        //public List<JokeLikesCounts> GetAll()
        //{
        //    var db = new UserJokeRepository(_conn);
        //    return db.GetLikeCounts();
        //}

        //EF approach
        [Route("getall")]
        [HttpGet]
        public List<JokeLikesCounts> GetAll()
        {
            var db = new UserJokeRepository(_conn);
            return db.GetJokesWithLikesCounts_EF().Select(j => new JokeLikesCounts
            {
                JokeId = j.Id,
                Question = j.Question,
                Punchline = j.Punchline,
                CountDislike = j.UserJokeLikes.Count(j => !j.Like),
                CountLike = j.UserJokeLikes.Count(j=> j.Like)
            }).ToList();
        }

        [Route("getuserlike")]
        [HttpGet]
        public UserJokeLike GetUserLike(int jokeId)
        {
            var user = GetCurrentUser();

            if (user is null)
            {
                return null;
            }

           var db = new UserJokeRepository(_conn);
           return db.GetUserJokeLike(jokeId, user.Id);
        }

        [Route("withinchangewindow")]
        public bool WithinWindowChangeLikeStatus(int jokeId)
        {
            var user = GetCurrentUser();

            if (user is null)
            {
                return false;
            }

            var db = new UserJokeRepository(_conn);
            var lastLike = (DateTime)db.WithinWindowToChangeLikeStatus(jokeId, user.Id);
            return  lastLike.AddMinutes(10) >= DateTime.Now;
         }


    }
}

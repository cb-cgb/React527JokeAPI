using System;
using System.Collections.Generic;
using System.Text;
using React527JokeAPI.data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace React527JokeAPI.data
{
    public class UserJokeRepository
    {
        private string _conn;
        public UserJokeRepository(string connection)
        {
            _conn = connection;
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new UserJokeContext(_conn))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public void AddUser(User u)
        {
            using (var context = new UserJokeContext(_conn))
            {

                //add only if userName doesn't already exist
                var user = GetUserByEmail(u.Email);

                if (user is null)
                {
                    u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);//encrypts the password value passed in
                    context.Add(u);
                    context.SaveChanges();
                 }
             }
         }


            public User Login(User u)
            {
                var user = GetUserByEmail(u.Email);

                if (user is null)
                {
                    return null;
                }

                var isValidPass = BCrypt.Net.BCrypt.Verify(u.Password, user.Password);

                if (!isValidPass)
                {
                    return null;
                }

                return user;
            }

        public void AddJoke(Joke j)
        {
            using (var context = new UserJokeContext(_conn))
            {
                context.Jokes.Add(j);
                context.SaveChanges();                
            }
        }

        public Joke GetJokeFromDB(int id)
        {
            using (var context = new UserJokeContext(_conn))
            {
                return context.Jokes.Include(j => j.UserJokeLikes).FirstOrDefault(j => j.ResultJokeId == id);
            }
        }
        public void AddJokeLike (int jokeId, int userId, bool like)
        {
            using (var context = new UserJokeContext(_conn))
            {
                var date = DateTime.Now;
                var uj = new UserJokeLike { JokeId = jokeId, UserId = userId, Date = date, Like = like };

                context.UserJokeLikes.Add(uj);
                context.SaveChanges();

                //context.Users.FirstOrDefault(u => u.Id == userId).UserJokeLikes.Add(uj);
                //context.Jokes.FirstOrDefault(j => j.Id == jokeId).UserJokeLikes.Add(uj);
                context.SaveChanges();
            }
        }

        public UserJokeLike GetUserJokeLike(int jokeId, int userId)
        {
            using (var context = new UserJokeContext(_conn))
            {
                return context.UserJokeLikes.FirstOrDefault(j => j.JokeId == jokeId && j.UserId == userId);
             }
        }

        public void AddUpdateJokeLike (int jokeId, int userId, bool like)
        {
           using (var context = new UserJokeContext(_conn))
            {

               var userLike = GetUserJokeLike(jokeId, userId);
               if (userLike is null)
                {
                    AddJokeLike(jokeId, userId, like);
                }

                else
                {
                    if (WithinWindowToChangeLikeStatus(jokeId, userId) != null)
                    {
                        //userLike.Date = DateTime.Now;
                        //userLike.Like = like;
                        var u = context.UserJokeLikes.FirstOrDefault(j => j.JokeId == jokeId && j.UserId == userId);
                        u.Date = DateTime.Now;
                        u.Like = like;
                        context.SaveChanges();
                    }                   
                }
            }
        }

        public List<JokeLikesCounts> GetLikeCounts()
        {
            using (var context = new UserJokeContext(_conn))
            {
                //var counts = new JokeLikesCounts();
                //counts.CountLike = context.UserJokeLikes.Select(j => j.JokeId == jokeId && j.Like).Count();
                //counts.CountDislike = context.UserJokeLikes.Select(j => j.JokeId == jokeId && !j.Like).Count();


                var query = @"SELECT counts.* , Question, Punchline 
                              FROM 
                              (
                                select isnull(dislikes.jokeId, likes.jokeId)jokeId,isnull(countDislikes,0)countDislikes,
                                       isnull(countLikes,0)countLikes 
                                from 
                                    (SELECT jokeId,count(*) countDislikes from UserJokeLikes where [Like]=0  GROUP BY jokeId,[Like]) as dislikes
                                 full outer join
                                    (SELECT jokeId,count(*) countLikes from UserJokeLikes where [Like]=1  GROUP BY jokeId,[Like]) as likes
                                  on likes.jokeId = dislikes.JokeId
                               ) counts, Jokes j
                              WHERE counts.jokeId = j.id";

                return context.JokeLikesCounts.FromSqlRaw(query).ToList();

            }
        }

        public List<Joke> GetJokesWithLikesCounts_EF()
        {
            using (var context = new UserJokeContext(_conn))
            {
                return context.Jokes.Include(j => j.UserJokeLikes).ToList();
            }
        }

        public DateTime WithinWindowToChangeLikeStatus (int jokeId,int userId)
        {
            using(var context = new UserJokeContext(_conn))
            {
                var lastAction = GetUserJokeLike(jokeId, userId);
                if (lastAction is null)
                {
                    return new DateTime(2999, 1, 1);
                }

                //var isPastWindow = DateTime.Now >= lastAction.Date.AddMinutes(10);
                //if (isPastWindow)
                //{
                //    return null;
                //}

                return lastAction.Date;
            }
        }

        public bool JokeExists(int jokeApiId)
        {
            using (var context = new UserJokeContext(_conn))
            {
                var exists =  context.Jokes.FirstOrDefault(j => j.ResultJokeId == jokeApiId) != null;
                return exists;
            }
        }

        //public  bool JokeExistsForUser(int jokeApiId,  int userId)
        //{
        //    using (var context = new UserJokeContext(_conn))
        //    {
        //        var jokeId = context.Jokes.FirstOrDefault(j => j.ResultJokeId == jokeApiId).Id;
        //        return GetUserJokeLike(jokeId,userId) != null;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace React527JokeAPI.data
{
    public class UserJokeContext : DbContext
    {

        private string _conn;

        public UserJokeContext(string connection)
        {
            _conn = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conn);
        }



        //many to many relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Taken from here:
            //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration

            //*********
            //this is needed to prevent cascade paths error creating the foreign key in ef
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //must add using System.Linq;
            //********

            //set up composite primary key
            modelBuilder.Entity<UserJokeLike>()
                .HasKey(uj => new { uj.UserId, uj.JokeId });

            //set up foreign key from UserJokes to Users
            modelBuilder.Entity<UserJokeLike>()
                .HasOne(uj => uj.User)
                .WithMany(u => u.UserJokeLikes)
                .HasForeignKey(u => u.UserId);

            //set up foreign key from UserJokes to Jokes
            modelBuilder.Entity<UserJokeLike>()
                .HasOne(uj => uj.Joke)
                .WithMany(j => j.UserJokeLikes)
                .HasForeignKey(j => j.JokeId);

            base.OnModelCreating(modelBuilder);

            //JokeLikeCounts is a custom class to use for a custom query. Don't want it created as a table in the db.
            modelBuilder.Entity<JokeLikesCounts>().HasNoKey().ToView(null);
            

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserJokeLike> UserJokeLikes { get; set; }
        public DbSet<JokeLikesCounts> JokeLikesCounts { get; set; }


    }
}

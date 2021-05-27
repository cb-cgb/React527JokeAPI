using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace React527JokeAPI.data
{
    public class UserJokeContextFactory : IDesignTimeDbContextFactory<UserJokeContext>
    {
        public UserJokeContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}React527JokeAPI.web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new UserJokeContext(config.GetConnectionString("ConStr"));
        }

    }
}
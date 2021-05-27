using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace React527JokeAPI.data
{
    public class UserJokeLike
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public DateTime Date { get; set; }
        public Boolean Like { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Joke Joke { get; set; }
    }
}

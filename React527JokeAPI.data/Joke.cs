using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace React527JokeAPI.data
{
    public class Joke
    {
        
        public int Id { get; set; }

        
        public int ResultJokeId { get; set; }
        
        
        public string Question { get; set; }
        public string Punchline { get; set; }

        public List<UserJokeLike>UserJokeLikes { get; set; }
    }
}

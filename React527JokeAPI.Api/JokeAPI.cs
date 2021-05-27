using System;
using System.Net.Http;
using React527JokeAPI.data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace React527JokeAPI.Api
{
    public static class JokeAPI
    {

        private class JokeFromAPI
        {
            public int Id { get; set; }

            [JsonProperty("setup")]
            public string Question { get; set; }

            public string Punchline { get; set; }
        }


        public static  Joke GetJoke()
        {
            var client = new HttpClient();
            var json = client.GetStringAsync($"https://official-joke-api.appspot.com/jokes/programming/random").Result;
            var joke =  JsonConvert.DeserializeObject<List<JokeFromAPI>>(json).FirstOrDefault();

            return new Joke
            {
                ResultJokeId = joke.Id,
                Question = joke.Question,
                Punchline = joke.Punchline
            };
        }
    }
}

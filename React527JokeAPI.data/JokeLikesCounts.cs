using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React527JokeAPI.data
{
    public class JokeLikesCounts
    {
        public int JokeId { get; set; }
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public string Question { get; set; }
        public string Punchline { get; set; }
    }
}

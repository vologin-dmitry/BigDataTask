using System;
using System.Collections.Generic;
using System.Text;

namespace BigDataTask
{
    public class Movie
    {
        public Movie()
        {
            Actors = new HashSet<string>();
            Tags = new HashSet<string>();
        }
        public String Name;
        public String Director;
        public String Rating;
        public HashSet<String> Actors;
        public HashSet<String> Tags;
    }
}

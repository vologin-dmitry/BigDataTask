using System.Linq;
using System.Collections.Generic;

namespace WebBigData
{
    public class DataBaseWorker
    {
        public List<Tag> GetTag(string name)
        {
                var toReturn = new List<Tag>();
                using (AppContext ac = new AppContext())
                {
                    IEnumerable<Tag> temp =
                                from tag in ac.Tags
                                where tag.Name == name
                                select tag;
                    toReturn = temp.ToList();
                }
                return toReturn;
        }

        public List<Movie> GetMovie(string name)
        {
            var toReturn = new List<Movie>();
            using (AppContext ac = new AppContext())
            {
                IEnumerable<Movie> temp =
                            from movie in ac.Movies
                            where movie.Name == name
                            select movie;
                toReturn = temp.ToList();
            }
            return toReturn;
        }

        public List<Person> GetPerson(string name)
        {
                var toReturn = new List<Person>();
                using (AppContext ac = new AppContext())
                {
                    IEnumerable<Person> temp =
                                from person in ac.Actors
                                where person.Name == name
                                select person;
                    toReturn = temp.ToList();
                }
                return toReturn;
        }
    }
}

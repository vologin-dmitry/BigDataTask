using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BigDataTask
{
    public class Reader
    {
        public Dictionary<String, List<String>> IdMovieByName;
        public Dictionary<String, List<Movie>> MovieByName;
        public Dictionary<String, Movie> MovieById;
        public Dictionary<String, String> NameByIdMovie;
        public Dictionary<String, String> NameByIdPerson;
        public Dictionary<String, String> LinksIMDB;
        public Dictionary<String, String> LinksTMDB;
        public Dictionary<String, String> CodeTag;
        public Dictionary<String, List<Movie>> TagMovie;
        public Dictionary<String, List<Movie>> MovieByPerson;
        private StreamReader sr;

        public Reader()
        {
            MovieById = new Dictionary<String, Movie>();
            IdMovieByName = new Dictionary<String, List<String>>();
            NameByIdMovie = new Dictionary<String, String>();
            MovieByName = new Dictionary<String, List<Movie>>();
            NameByIdPerson = new Dictionary<String, String>();
            LinksIMDB = new Dictionary<String, String>();
            LinksTMDB = new Dictionary<String, String>();
            CodeTag = new Dictionary<String, String>();
            TagMovie = new Dictionary<String, List<Movie>>();
            MovieByPerson = new Dictionary<String, List<Movie>>();
        }

        public void Initialize()
        {
            ReadMovieCodes();
            ActorsDirectorsNameRead();
            ActorsDirectorsCodesRead();
            RatingsRead();
            LinksRead();
            TagCodes();
            TagScores();
        }

        public void ReadMovieCodes()
        {
            sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\MovieCodes_IMDB.tsv");
            String data;
            _ = sr.ReadLine();
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (parsed[3] == "RU" || parsed[3] == "US")
                {
                    if (!NameByIdMovie.ContainsKey(parsed[0]))
                    {
                        NameByIdMovie.Add(parsed[0], parsed[2]);
                        if (!IdMovieByName.ContainsKey(parsed[2]))
                        {
                            var ids = new List<String>();
                            var movies = new List<Movie>();
                            ids.Add(parsed[0]);
                            var mov = new Movie { Name = parsed[2] };
                            movies.Add(mov);
                            IdMovieByName.Add(parsed[2], ids);
                            MovieByName.Add(parsed[2], movies);
                            MovieById.Add(parsed[0], mov);
                        }
                        else
                        {
                            IdMovieByName[parsed[2]].Add(parsed[0]);
                            var mov = new Movie { Name = parsed[2] };
                            MovieById.Add(parsed[0], mov);
                            MovieByName[parsed[2]].Add(mov);
                        }
                    }

                }
            }
        }

        public void ActorsDirectorsNameRead()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\ActorsDirectorsNames_IMDB.txt");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (!NameByIdPerson.ContainsKey(parsed[0]))
                {
                    NameByIdPerson.Add(parsed[0], parsed[1]);
                }
            }
        }

        public void ActorsDirectorsCodesRead()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\ActorsDirectorsCodes_IMDB.tsv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (NameByIdMovie.ContainsKey(parsed[0]))
                {
                    if (NameByIdPerson.ContainsKey(parsed[2]))
                    {
                        var name = NameByIdPerson[parsed[2]];
                        if (MovieByPerson.ContainsKey(name))
                        {
                            var movies = MovieByPerson[name];
                            movies.Add(MovieById[parsed[0]]);
                        }
                        else
                        {
                            var mov = new List<Movie>();
                            mov.Add(MovieById[parsed[0]]);
                            MovieByPerson.Add(name, mov);
                        }
                        if (parsed[4] == "actor" || parsed[4] == "actress" || parsed[3] == "actor" || parsed[3] == "actress")
                        {
                            MovieById[parsed[0]].Actors.Add(NameByIdPerson[parsed[2]]);
                        }
                        if (parsed[4] == "director" || parsed[3] == "director")
                        {
                            MovieById[parsed[0]].Director = NameByIdPerson[parsed[2]];
                        }
                    }
                }
            }
        }

        public void RatingsRead()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\Ratings_IMDB.tsv");
            var title = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (MovieById.ContainsKey(parsed[0]))
                {
                    MovieById[parsed[0]].Rating = parsed[1];
                }
            }
        }

        public void LinksRead()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\links_IMDB_MovieLens.csv");
            var title = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                //IMDB-TMDB
                LinksIMDB.Add(parsed[0], parsed[1]);
                LinksTMDB.Add(parsed[0], parsed[2]);
            }
        }

        public void TagScores()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\TagScores_MovieLens.csv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                if (parsed[2][2] == '5' || parsed[2][2] == '6' || parsed[2][2] == '7' || parsed[2][2] == '8' || parsed[2][2] == '9')
                {
                    var codeIMDB = "tt" + LinksIMDB[parsed[0]];
                    if (MovieById.ContainsKey(codeIMDB))
                    {
                        var mov = MovieById[codeIMDB];
                        mov.Tags.Add(CodeTag[parsed[1]]);
                        if (TagMovie.ContainsKey(CodeTag[parsed[1]]))
                        {
                            TagMovie[CodeTag[parsed[1]]].Add(mov);
                        }
                        else
                        {
                            var d = new List<Movie>();
                            d.Add(mov);
                            TagMovie.Add(CodeTag[parsed[1]], d);
                        }
                    }
                }
            }
        }

        public void TagCodes()
        {
            StreamReader sr = new StreamReader(@"E:\Programs\reps\BigDataTask\BigDataTask\TagCodes_MovieLens.csv");
            var title = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                CodeTag.Add(parsed[0], parsed[1]);
            }
        }
    }
}

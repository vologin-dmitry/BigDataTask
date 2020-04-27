using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WebBigData
{
    static public class DataBaseReader
    {
        static public Dictionary<String, List<String>> NameId_mov;
        static public Dictionary<String, List<Movie>> NameMovie_mov;
        static public Dictionary<String, Movie> MovieId_mov;
        static public Dictionary<String, String> IdName_mov;
        static public Dictionary<String, String> IdName_pers;
        static public Dictionary<String, String> LinksIMDB;
        static Dictionary<String, String> CodeTag;
        static public Dictionary<String, List<Movie>> TagMovie;
        static public Dictionary<String, List<Movie>> MovieByPerson;

        static private StreamReader sr;

        static DataBaseReader()
        {
            MovieId_mov = new Dictionary<String, Movie>();
            NameId_mov = new Dictionary<String, List<String>>();
            IdName_mov = new Dictionary<String, String>();
            NameMovie_mov = new Dictionary<String, List<Movie>>();
            IdName_pers = new Dictionary<String, String>();
            LinksIMDB = new Dictionary<String, String>();
            CodeTag = new Dictionary<String, String>();
            TagMovie = new Dictionary<String, List<Movie>>();
            MovieByPerson = new Dictionary<String, List<Movie>>();
        }

        static public void ReinitDatabase()
        {
            ReadMovieCodes();
            ActorsDirectorsNameRead();
            ActorsDirectorsCodesRead();
            RatingsRead();
            LinksRead();
            TagCodes();
            TagScores();
            TMBDRead();
            RecreateBD();
            CreateMovieList();
            CreateActorList();
            CreateTagList();
        }

        static private void ReadMovieCodes()
        {
            sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\MovieCodes_IMDB.tsv");
            String data;
            _ = sr.ReadLine();
            int count = 0;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (parsed[3] == "RU" || parsed[3] == "US")
                {
                    if (!IdName_mov.ContainsKey(parsed[0]))
                    {
                        IdName_mov.Add(parsed[0], parsed[2]);
                        if (!NameId_mov.ContainsKey(parsed[2]))
                        {
                            var ids = new List<String>();
                            var movies = new List<Movie>();
                            ids.Add(parsed[0]);
                            var mov = new Movie(parsed[2], parsed[0]);
                            movies.Add(mov);
                            NameId_mov.Add(parsed[2], ids);
                            NameMovie_mov.Add(parsed[2], movies);
                            MovieId_mov.Add(parsed[0], mov);
                        }
                        else
                        {
                            NameId_mov[parsed[2]].Add(parsed[0]);
                            var mov = new Movie(parsed[2], parsed[0]);
                            MovieId_mov.Add(parsed[0], mov);
                            NameMovie_mov[parsed[2]].Add(mov);
                        }
                        ++count;
                    }

                }
            }
        }

        static private void ActorsDirectorsNameRead()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\ActorsDirectorsNames_IMDB.txt");
            _ = sr.ReadLine();
            String data;
            int count = 0;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (!IdName_pers.ContainsKey(parsed[0]))
                {
                    IdName_pers.Add(parsed[0], parsed[1]);
                    ++count;
                }
            }
        }

        static private void ActorsDirectorsCodesRead()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\ActorsDirectorsCodes_IMDB.tsv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (IdName_mov.ContainsKey(parsed[0]))
                {
                    if ((IdName_pers.ContainsKey(parsed[2])) && (parsed[4] == "actor" || parsed[4] == "actress" ||
                        parsed[3] == "actor" || parsed[3] == "actress" ||
                        parsed[4] == "director" || parsed[3] == "director"))
                    {
                        var name = IdName_pers[parsed[2]];
                        if (MovieByPerson.ContainsKey(name))
                        {
                            var movies = MovieByPerson[name];
                            movies.Add(MovieId_mov[parsed[0]]);
                        }
                        else
                        {
                            var mov = new List<Movie>();
                            mov.Add(MovieId_mov[parsed[0]]);
                            MovieByPerson.Add(name, mov);
                        }
                        if (parsed[4] == "actor" || parsed[4] == "actress" || parsed[3] == "actor" || parsed[3] == "actress")
                        {
                            MovieId_mov[parsed[0]].AddActor(IdName_pers[parsed[2]]);
                        }
                        if (parsed[4] == "director" || parsed[3] == "director")
                        {
                            MovieId_mov[parsed[0]].Director = IdName_pers[parsed[2]];
                        }
                    }
                }
            }
        }

        static private void RatingsRead()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\Ratings_IMDB.tsv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split('\t');
                if (MovieId_mov.ContainsKey(parsed[0]))
                {
                    MovieId_mov[parsed[0]].Rating = parsed[1];
                }
            }
        }

        static public void LinksRead()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\links_IMDB_MovieLens.csv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                LinksIMDB.Add("tt" + parsed[1], parsed[2]);
            }
        }

        static public void TMBDRead()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\links_IMDB_MovieLens.csv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                parsed[1] = "tt" + parsed[1];
                if (MovieId_mov.ContainsKey(parsed[1]))
                {
                    MovieId_mov[parsed[1]].TMBD = parsed[2];
                }
            }
        }

        static private void TagScores()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\TagScores_MovieLens.csv");
            _ = sr.ReadLine();
            String data;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                if (parsed[2][2] == '5' || parsed[2][2] == '6' || parsed[2][2] == '7' || parsed[2][2] == '8' || parsed[2][2] == '9')
                {
                    var codeIMDB = "tt" + LinksIMDB[parsed[0]];
                    if (MovieId_mov.ContainsKey(codeIMDB))
                    {
                        var mov = MovieId_mov[codeIMDB];
                        mov.AddTag(CodeTag[parsed[1]]);
                        if (TagMovie.ContainsKey(CodeTag[parsed[1]]))
                        {
                            TagMovie[CodeTag[parsed[1]]].Add(mov);
                        }
                        else
                        {
                            var movieList = new List<Movie>();
                            movieList.Add(mov);
                            TagMovie.Add(CodeTag[parsed[1]], movieList);
                        }
                    }
                }
            }
        }

        static private void TagCodes()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Дмитрий\Desktop\\WebBigData\WebBigData\TagCodes_MovieLens.csv");
            _ = sr.ReadLine();
            String data;
            int count = 0;
            while (!((data = sr.ReadLine()) == null))
            {
                var parsed = data.Split(',');
                CodeTag.Add(parsed[0], parsed[1]);
                ++count;
            }
        }

        static private void RecreateBD()
        {
            NameId_mov.Clear();
            IdName_mov.Clear();
            LinksIMDB.Clear();
            MovieId_mov.Clear();
            using (AppContext ac = new AppContext())
            {
                ac.ReCreate();
            }
        }

        static private void CreateMovieList()
        {
            int count = 0;
            int iter = 0;
            using (AppContext ac = new AppContext())
            {
                foreach (var nameMovie in NameMovie_mov)
                {
                    if (count * iter <= 400000)
                    {
                        foreach (var mov in nameMovie.Value)
                        {
                            GetRecommendedFilms(mov);
                            ac.Movies.Add(mov);
                            ++count;
                        }
                        if (count >= 100)
                        {
                            ac.SaveChanges();
                            count = 0;
                            iter++;
                        }
                    }
                }
            }
            NameMovie_mov.Clear();
        }

        static private void CreateActorList()
        {
            using (AppContext ac = new AppContext())
            {

                int count = 0;
                foreach (var person in IdName_pers)
                {
                    if (MovieByPerson.ContainsKey(person.Value))
                    {
                        var actor = new Person(person.Value, person.Key);
                        var movList = new List<Movie>();
                        movList = MovieByPerson[person.Value];
                        foreach (var mov in movList)
                        {
                            actor.AddFilm(mov.Name);
                            ++count;
                        }
                        ac.Actors.Add(actor);
                        if (count >= 6400)
                        {
                            count = 0;
                            ac.SaveChanges();
                        }
                    } 
                }
            }
            IdName_pers.Clear();
            MovieByPerson.Clear();
        }

        static private void CreateTagList()
        {
            using (AppContext ac = new AppContext())
            {
                int count = 0;
                foreach (var tag in CodeTag)
                {
                    var tagToAdd = new Tag(tag.Value);
                    var movList = new List<Movie>();
                    if (TagMovie.ContainsKey(tag.Value))
                    {
                        movList = TagMovie[tag.Value];
                    }
                    foreach (var mov in movList)
                    {
                        tagToAdd.AddFilm(mov.Name);
                        ++count;
                    }
                    ac.Tags.Add(tagToAdd);
                    if (count >= 6400)
                    {
                        count = 0;
                        ac.SaveChanges();
                    }
                }
            }
            CodeTag.Clear();
            TagMovie.Clear();
        }

        static public void GetRecommendedFilms(Movie movie)
        {
            var toSort = new List<(int, String, String)>();
            foreach (var actor in movie.Actors.Split("\n"))
            {
                if (MovieByPerson.ContainsKey(actor))
                {
                    foreach (var toCompare in MovieByPerson[actor])
                    {
                        toSort.Add((CompareTwoFilms(movie, toCompare), toCompare.TMBD, toCompare.Name));
                    }
                }
            }
            foreach (var tag in movie.Tags.Split("\n"))
            {
                if (TagMovie.ContainsKey(tag))
                {
                    foreach (var toCompare in TagMovie[tag])
                    {
                        toSort.Add((CompareTwoFilms(movie, toCompare), toCompare.TMBD, toCompare.Name));
                    }
                }
            }
            toSort.Sort(Comparer<(int, String, String)>.Create(new Comparison<(int, String, String)>((x,y)=>y.Item1 - x.Item1)));
            for (int j = 0, i = 0; i < 10 && j < toSort.Count; ++j)
            {
                if (toSort[j].Item2 != movie.TMBD)
                {
                    if (toSort[j].Item2 != null && toSort[j].Item2 != "")
                    {
                        movie.AddSimiliar(toSort[j].Item2);
                        ++i;
                    }
                    ++j;
                }
            }

        }

        static private int CompareTwoFilms(Movie first, Movie second)
        {
            int rating = 0;
            foreach (var actorFirst in first.Actors.Split("\n"))
            {
                foreach (var actorSecond in second.Actors.Split("\n"))
                {
                    if (actorFirst == actorSecond)
                    {
                        rating += 1;
                    }
                }
            }
            foreach (var tagFirst in first.Tags.Split("\n"))
            {
                foreach (var tagSecond in second.Tags.Split("\n"))
                {
                    if (tagFirst == tagSecond)
                    {
                        rating += 3;
                    }
                }
            }
            if (first.Director == second.Director)
            {
                rating += 5;
            }
            return rating - 2;
        }
    }

}
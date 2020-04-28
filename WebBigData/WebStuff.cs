using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace WebBigData
{
    public class WebStuff
    {
        public string Info { get; set; }
        public string Image { get; set; }
        public List<(string, string)> alike { get; set; }
        private string imagePrefix = "https://image.tmdb.org/t/p/w200";
        private Movie movie;

        public WebStuff(Movie mov)
        {
            alike = new List<(string, string)>();
            this.movie = mov;
            GetInfo();
            Alike();
        }

        private void GetInfo()
        {
            var uri = "https://api.themoviedb.org/3/movie/" + movie.TMBD + "?api_key=12cc22e510ea8204c404498269c06dee&language=en-US";
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                var splitted = responseFromServer.Split("\"overview\":\"");
                Info = splitted[1].Split("\"popularity\"")[0];
                var splittedSec = responseFromServer.Split("\"poster_path\":\"");
                if (splittedSec.Length != 1)
                {
                    string imageName = splittedSec[1].Split("\"")[0];
                    Image = imagePrefix + imageName;
                }
            }
        }

        private void Alike()
        {
            var movies = movie.Recomended.Split("\t");
            if (movie.Recomended != "")
            {
                for (var i = 1; i < 11 && movies[i] != "" && i < movies.Length; ++i)
                {
                    var movUri = "https://api.themoviedb.org/3/movie/" +
                        movies[i] +
                        "?api_key=12cc22e510ea8204c404498269c06dee&language=en-US";
                    var request = (HttpWebRequest)WebRequest.Create(movUri);
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();
                        reader.Close();
                        dataStream.Close();
                        response.Close();
                        var splitted = responseFromServer.Split("\"original_title\":\"");
                        var itsTitle = splitted[1].Split("\"");
                        var splittedSec = responseFromServer.Split("\"poster_path\":\"");
                        string imageName = "";
                        if (splittedSec.Length != 1)
                        {
                            imageName = splittedSec[1].Split("\"")[0];
                        }
                        if (imageName != "")
                        {
                            alike.Add((itsTitle[0], imagePrefix + imageName));
                        }
                    }
                }
            }
        }
    }
}

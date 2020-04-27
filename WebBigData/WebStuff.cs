using System;
using System.Net;
using System.IO;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using System.Runtime;
using Microsoft.AspNetCore.Html;

namespace WebBigData
{
    public class WebStuff
    {
        private HttpWebRequest request;
        private HttpWebResponse response;
        private string info = "";
        private string imagePrefix = "https://image.tmdb.org/t/p/w200";
        private string image = "";
        private string alike = "/similar?api_key=12cc22e510ea8204c404498269c06dee&language=en-US&page=1";
        private Movie movie;

        public WebStuff(Movie mov)
        {
            this.movie = mov;
        }

        public bool MakeRequest(string uri)
        {
            request = (HttpWebRequest)WebRequest.Create(uri);
            response = (HttpWebResponse)request.GetResponse();
            return response.StatusCode == HttpStatusCode.OK;
        }

        public String GetInfo()
        {
            if (this.info != "")
            {
                return this.info;
            }
            var uri = "https://api.themoviedb.org/3/movie/" + movie.TMBD + "?api_key=12cc22e510ea8204c404498269c06dee&language=en-US";
            string info;
            if (MakeRequest(uri))
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                var splitted = responseFromServer.Split("\"overview\":\"");
                info = splitted[1].Split("\"popularity\"")[0];
                var splittedSec = responseFromServer.Split("\"poster_path\":\"");
                if (splittedSec.Length != 1)
                {
                    string imageName = splittedSec[1].Split("\"")[0];
                    image = imagePrefix + imageName;
                }
                this.info = info;
                return info;
            }
            return "Description not found";
        }

        public string Alike()
        {
            var toReturn = "";
            var movies = movie.Recomended.Split("\t");
            if (movie.Recomended != "")
            {
                for (var i = 1; i < 11 && movies[i] != "" && i < movies.Length; ++i)
                {
                    var movUri = "https://api.themoviedb.org/3/movie/" +
                        movies[i] +
                        "?api_key=12cc22e510ea8204c404498269c06dee&language=en-US";
                    request = (HttpWebRequest)WebRequest.Create(movUri);
                    response = (HttpWebResponse)request.GetResponse();
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
                        if (splittedSec.Length != 1)
                        {
                            string imageName = splittedSec[1].Split("\"")[0];
                            toReturn += imagePrefix + imageName + "\t";
                        }
                        toReturn += itsTitle[0] + "\n";
                    }
                }
            }
            return toReturn;
        }

        public String GetImage()
        {
            GetInfo();
            return image;
        }
    }
}

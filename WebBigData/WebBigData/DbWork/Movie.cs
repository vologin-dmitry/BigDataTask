using System;

namespace WebBigData
{
    public class Movie
    {
        public String Code { get; set; }

        public int Id { get; set; }
        public String TMBD { get; set; }
        public String Name { get; set; }
        public String Rating { get; set; }
        public String Director { get; set; }
        public String Actors { get; set; }
        public String Tags { get; set; }
        public String Recomended { get; set; }

        public Movie(String Name, string Code)
        {
            this.Name = Name;
            this.Code = Code;
            Rating = "";
            Director = "";
            Actors = "";
            Tags = "";
        }

        public void AddActor(String Actor)
        {
            Actors += Actor + "\t";
        }

        public void AddSimiliar(String Film)
        {
            Recomended += Film + "\t";
        }

        public void AddTag(String Tag)
        {
            Tags += Tag + "\t";
        }

        public void Print()
        {
            Tags.Replace('\n', '\t');
            Console.WriteLine(Name + '\t' + Code + '\t' + Director + '\n' + 
                "Rating: " + Rating + '\n' + Tags + '\n' + Actors);
        }
    }
}

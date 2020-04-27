using System;

namespace WebBigData
{
    public class Tag
    {
        public String Name { get; set; }
        public int Id { get; set; }
        public String Films { get; set; }

        public Tag(String Name)
        {
            this.Name = Name;
            Films = "";
        }

        public void AddFilm(String Film)
        {
            Films += Film + "\t";
        }

        public void Print()
        {
            Console.WriteLine(Name + '\t' + Id + '\n' + Films);
        }
    }
}

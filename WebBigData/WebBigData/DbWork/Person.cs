using System;

namespace WebBigData
{
    public class Person
    {
        public String Name { get; private set; }
        public int Id { get; private set; }
        public String Code { get; private set; }
        public String Films { get; private set; }

        public Person(String Name, String Code, String Films)
        {
            this.Name = Name;
            this.Code = Code;
            this.Films = Films;
        }

        public Person(String Name, String Code)
        {
            this.Name = Name;
            this.Code = Code;
            Films = "";
        }

        public void AddFilm(String Film)
        {
            this.Films += Film + "\t";
        }

        public void Print()
        {
            Console.WriteLine(Name + '\t' + Id + '\n' + "Films list: " + '\n' + Films);
        }
    }
}

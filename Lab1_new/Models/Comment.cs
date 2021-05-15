using System;
namespace Lab1_new.Models
{
    public class Comment

    {

        public long Id { get; set; }
        public string Text { get; set; }
        public bool Important{ get; set; }
        public Movie movie;

public Comment()
        {
        }
    }
}

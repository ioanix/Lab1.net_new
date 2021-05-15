using System;
using System.Collections.Generic;
using Lab1_new.Models;

namespace Lab1_new.ViewModels
{
    public class MovieViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Genre Genre { get; set; }

        public int Duration { get; set; }

        public int YearOfRelease { get; set; }

        public string Director { get; set; }

        public DateTime DateAdded { get; set; }

        public int Rating { get; set; }

        public string Watched { get; set; }

        public List<Comment> CommentsList { get; set; }


        public MovieViewModel()
        {

        
        }
    }
}

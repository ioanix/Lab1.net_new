using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_new.Models
{
    public class Movie
    {

        public int Id { get; set; }

        //[Required]
        public string Title { get; set; }

        //[Required]
        public string Description { get; set; }

        //[Required]
        public Genre Genre { get; set; }

        //[Required]
        public int Duration { get; set; }

        //[Required]
        public int YearOfRelease { get; set; }

        //[Required]
        public string Director { get; set; }

        //[Required]
        public DateTime DateAdded { get; set; }

        //[Range(1, 10)]
        public int Rating { get; set; }

        public string Watched { get; set; }

        public List<Comment> CommentsList { get; set; }

        public List<Reservation> Reservations { get; set; }

        public Movie()
        {
        }
    }


    public enum Genre
    {
        Action,
        Comedy,
        Horror,
        Thriller
    }
}

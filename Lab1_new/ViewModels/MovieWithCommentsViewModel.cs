using System;
using System.Collections.Generic;

namespace Lab1_new.ViewModels
{
    public class MovieWithCommentsViewModel

    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Watched { get; set; }
        public IEnumerable<CommentViewModel> CommentsList { get; set; }


        public MovieWithCommentsViewModel()
        {
        }
    }
}

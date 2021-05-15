using System;
namespace Lab1_new.ViewModels
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        

        public CommentViewModel()
        {
        }
    }
}

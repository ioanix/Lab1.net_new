using System;
using AutoMapper;
using Lab1_new.Models;
using Lab1_new.ViewModels;

namespace Lab1_new
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Movie, MovieViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Movie, MovieWithCommentsViewModel>();

        }
    }
}

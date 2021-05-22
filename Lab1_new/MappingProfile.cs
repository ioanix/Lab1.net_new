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
            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Movie, MovieWithCommentsViewModel>().ReverseMap();

        }
    }
}

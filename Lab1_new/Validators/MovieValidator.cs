using System;
using FluentValidation;
using Lab1_new.ViewModels;

namespace Lab1_new.Validators
{
    public class MovieValidator: AbstractValidator<MovieViewModel>
    {
        public MovieValidator()
        {
            RuleFor(m => m.Description).MinimumLength(10);
            RuleFor(m => m.Rating).InclusiveBetween(1, 10);
        }
    }
}

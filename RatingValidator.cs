using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VanitiProject.Models;

namespace VanitiProject
{
    /// <summary>
    /// A validator class for validating the properties of a Rating.
    /// </summary>
    public class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(r => r.UserName).NotEmpty();
            RuleFor(r => r.RatingValue).InclusiveBetween(1, 5);
        }
    }

}
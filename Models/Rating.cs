using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VanitiProject.Models
{
    /// <summary>
    /// Rating
    /// </summary>
    public class Rating
    {
        public string UserName { get; set; }

        public int RatingValue { get; set; }

        public string Comments { get; set; }

    }
}
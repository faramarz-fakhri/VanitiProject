using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VanitiProject.Models
{
    public class UserRatingWrapper
    {
        public int BeerId { get; set; }
        public Rating Rating { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VanitiProject.Models
{
    /// <summary>
    /// Beer Class
    /// </summary>
    public class Beer
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Tagline { get; set; }

        public string ImageUrl { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2_1_25_Question2.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }
        public string Moviename { get; set; }
        public DateTime DateofRelease { get; set; }
    }
}
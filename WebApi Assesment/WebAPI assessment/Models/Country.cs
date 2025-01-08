using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_assessment.Models
{
    public class Country
    {
        public int ID {  get; set; }
        public string Cname { get; set; }
        public string capital { get; set; }
        
        public Country(int ID, string cname, string capital)
        {
            this.ID = ID;
            this.capital = capital;
            this.Cname = cname;
        }
    }
}
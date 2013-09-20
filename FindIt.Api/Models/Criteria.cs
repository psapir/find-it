using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindIt.Api.Models
{
    public class Criteria
    {
        public String Mid { get; set; }
        public String Keyword { get; set;} 
        public bool CaseSensitive{ get; set; }
    }
}
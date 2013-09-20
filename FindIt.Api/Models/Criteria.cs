using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindIt.Api.Models
{
    public class ItemType {
        public String TypeName { get; set; }
    }
    public class Criteria
    {
        public String Mid { get; set; }
        public String Keyword { get; set;} 
        public bool CaseSensitive{ get; set; }
        public List<ItemType> Type { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
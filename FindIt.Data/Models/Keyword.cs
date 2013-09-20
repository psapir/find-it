using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindIt.Api.Models
{
    public class Keyword
    {
        [Key]
        public Guid IdKeyword { get; set; }

        [StringLength(1000)]
        public String KeywordText { get; set; }

    }
}
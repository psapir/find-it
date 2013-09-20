using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindIt.Api.Models
{
    public class ContactIndex
    {
        [Key]
        public Guid IdContactIndex { get; set; }

        [StringLength(150)]
        public String mid { get; set; }

        public DateTime LastIndexRun { get; set; }

    }
}
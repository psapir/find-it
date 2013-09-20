using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace FindIt.Api.Models
{
    public enum ResultType
    {
        email,
        dataExtension,
        contentArea, 
        portfolioItem
    }

    public class Result
    {
        [Key]
        public Guid IdResult { get; set; }
        [StringLength(500)]
        public String CustomerKey { get; set; }
        [StringLength(500)]
        public String Name { get; set; }
        [StringLength(200)]
        public String ResultType { get; set; }
        public String Path { get; set; }
        [StringLength(1000)]
        public String URL { get; set; }
        [StringLength(1000)]
        public String ThumbnailURL { get; set; }
        public DateTime CreatedDate{ get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid IdContactIndex { get; set; }

        [ForeignKey("IdContactIndex")]
        public ContactIndex ContactIndex { get; set; }

        
    }
}
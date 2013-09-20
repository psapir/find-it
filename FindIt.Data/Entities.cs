using FindIt.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FindIt.Data
{
    public class Entities: DbContext
    {
        public DbSet<Result> Results { get; set; }
        public DbSet<ContactIndex> ContactIndexes { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
   }
}
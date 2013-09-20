using FindIt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindIt.Api.Controllers
{
    public class FindItController : ApiController
    {
        public IEnumerable<object> Find(dynamic criteria)
        {
            List<object> results = new List<object>();

            using (Entities db = new Entities())
            { 
                
            }

            return results;
        }
    }
}

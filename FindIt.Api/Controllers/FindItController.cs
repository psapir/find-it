using FindIt.Api.Models;
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
        // /api/find
        public IEnumerable<object> Find(Criteria criteria)
        {
            List<object> results = new List<object>();

            // split keywords by comma will search for multiple keywords.
            if (criteria.CaseSensitive)
                criteria.Keyword = criteria.Keyword.ToLower();

            List<string> keywords = criteria.Keyword.Split(',').ToList();

            using (Entities db = new Entities())
            { 
                db.Results.Where(r => r.ContactIndex.mid == criteria.Mid && 
                    criteria.Type.Exists(t => t.TypeName.ToLower() == r.ResultType.ToString().ToLower()) && 
                    r.Keywords.Exists(k => keywords.Contains(criteria.CaseSensitive ? k.KeywordText.ToLower() : k.KeywordText)))
                    .Skip(criteria.PageNumber*criteria.ItemsPerPage).Take(criteria.ItemsPerPage);     
            }

            return results;
        }
    }
}

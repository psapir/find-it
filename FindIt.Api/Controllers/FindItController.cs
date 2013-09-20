﻿using FindIt.Api.Models;
using FindIt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FindIt.Api.Controllers
{
    public class FindItController : ApiController
    {

        // /api/find
        public IEnumerable<object> Find(Criteria criteria)
        {
            List<object> results = new List<object>();

            List<Result> searchResults = new List<Result>();

            // split keywords by comma will search for multiple keywords.
            if (criteria.CaseSensitive)
                criteria.Keyword = criteria.Keyword.ToLower();

            List<string> keywords = criteria.Keyword.Split(',').ToList();

            using (Entities db = new Entities())
            { 
                /*searchResults = db.Results.Where(r => r.ContactIndex.mid == criteria.Mid && 
                    criteria.Type.FirstOrDefault(t => t.ToLower() == r.ResultType.ToLower()) != null &&
                    r.Keywords.FirstOrDefault(k => keywords.Contains(criteria.CaseSensitive ? k.KeywordText.ToLower() : k.KeywordText)) != null)
                    .OrderBy(r=>r.ModifiedDate).Skip(criteria.PageNumber*criteria.ItemsPerPage).Take(criteria.ItemsPerPage).ToList();*/
                searchResults = db.Results.ToList();
            }
            
            return searchResults;
        }
    }
}


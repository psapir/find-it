using FindIt.Api.Models;
using FindIt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindIt.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (Entities e = new Entities()) {

                ContactIndex ci = new Models.ContactIndex() { IdContactIndex = Guid.NewGuid(), LastIndexRun = DateTime.Now, mid = "12355" };

                e.ContactIndexes.Add(ci);

                List<Keyword> keywords = new List<Keyword>();

                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="pato"});
                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="et"});
                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="test"});
                e.SaveChanges();

                for (int i = 0; i < 300; i++)
			    {
                    var path = "my emails/path/" + i.ToString();
                    var res = new Result() { IdResult = Guid.NewGuid(), ContactIndex = ci, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, ResultType = ResultType.email, CustomerKey = Guid.NewGuid().ToString(), Path = path, Keywords = e.Keywords.ToList()};
			        e.Results.Add(res);
			    }
                
                e.SaveChanges();
            }
            return View();
        }
    }
}

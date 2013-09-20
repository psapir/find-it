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

                ContactIndex ci = new Models.ContactIndex() { IdContactIndex = Guid.NewGuid(), LastIndexRun = DateTime.Now, mid = "12356" };

                e.ContactIndexes.Add(ci);

                List<Keyword> keywords = new List<Keyword>();

                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="pato"});
                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="et"});
                keywords.Add(new Keyword(){IdKeyword=Guid.NewGuid(),KeywordText="test"});
                e.SaveChanges();

                for (int i = 0; i < 100; i++)
			    {
                    var path = "my emails/path/" + i.ToString();
                    var res = new Result() { Name= "email-"+i,IdResult = Guid.NewGuid(), ContactIndex = ci, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, ResultType = "email", CustomerKey = Guid.NewGuid().ToString(), Path = path, Keywords = e.Keywords.ToList()};
			        e.Results.Add(res);
			    }

                for (int i = 0; i < 100; i++)
                {
                    var path = "data extensions/path/" + i.ToString();
                    var res = new Result() { Name = "de-" + i, IdResult = Guid.NewGuid(), ContactIndex = ci, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, ResultType = "dataExtension", CustomerKey = Guid.NewGuid().ToString(), Path = path, Keywords = e.Keywords.ToList() };
                    e.Results.Add(res);
                }
                
                e.SaveChanges();
            }
            return View();
        }
    }
}

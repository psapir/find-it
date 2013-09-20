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
                e.ContactIndexes.Add(new Models.ContactIndex(){IdContactIndex = Guid.NewGuid(),LastIndexRun=DateTime.Now,mid="1234"});
                e.SaveChanges();
            }
            return View();
        }
    }
}

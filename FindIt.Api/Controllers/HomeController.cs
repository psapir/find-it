using FindIt.Api.Models;
using FindIt.Data;
using FuelSDK;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindIt.Api.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET /Home/Login called by IMH SSO 
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public RedirectResult Login(string jwt)
        {
            try
            {
                if (jwt.Trim().Length > 0)
                {
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("jwt", jwt);

                    ET_Client client = new ET_Client(parameters);

                    Session["client"] = client;

                    // Redirect to the application redirectURL specified in App Center.
                    return new RedirectResult("/home/index");
                }

                return new RedirectResult(Request.Url.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}

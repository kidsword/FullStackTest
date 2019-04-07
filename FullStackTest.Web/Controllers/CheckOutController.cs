using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FullStackTest.Web.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        [AllowCrossSite]
        public ActionResult Index()
        {
            
            return View();
        }
    }
}
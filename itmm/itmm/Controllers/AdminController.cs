using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itmm.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            ViewBag.MyMessage = "Welcome to administrator page <- Message brought to you by ViewBag";
            return View();
        }

    }
}

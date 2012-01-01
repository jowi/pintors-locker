using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itmm.Controllers
{
    public class StaffController : Controller
    {
        //
        // GET: /Staff/
        [Authorize(Roles="Staff")]
        public ActionResult Index()
        {
            return View();
        }

    }
}

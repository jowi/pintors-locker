using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace itmm.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            ViewBag.Message = "It seems that you don't have the authority to access that page (^_^)";
            return View();
        }
        public ActionResult GoNow()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Dev"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (Roles.IsUserInRole(User.Identity.Name, "Head"))
            {
                return RedirectToAction("Index", "Head");
            }
            else if (Roles.IsUserInRole(User.Identity.Name, "Staff"))
            {
                return RedirectToAction("Index", "Staff");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

    }
}

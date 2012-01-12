using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;

namespace itmm.Controllers
{
    public class HomeController : Controller
    {
        public pintorEntities con = new pintorEntities();
        public ActionResult Index(int? lab)
        {

            var x = from y in con.Laboratories
                    orderby y.LaboratoryName ascending
                    select y;
            ViewBag.LabList = x;

            var s = from y in con.Laboratory_Staff
                    where y.LaboratoryId == lab
                    select y;
            ViewBag.StaffList = s;


            ViewBag.Message = "Welcome!";

            if (!lab.Equals(null))
            {
                var a = from y in con.Laboratories
                        where y.LaboratoryId == lab
                        select y;
                ViewBag.LabInfo = a;
            }
     
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}

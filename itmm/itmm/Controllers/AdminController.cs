using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;

namespace itmm.Controllers
{
    public class AdminController : Controller
    {
        public pintorEntities con = new pintorEntities();
        //
        // GET: /Admin/
        [Authorize(Roles="Dev")]
        public ActionResult Index()
        {
            ViewBag.MyMessage = "Welcome to administrator page <- Message brought to you by ViewBag";

            var room = from y in con.Rooms
                       select y;
            ViewBag.Room = room;

            var lab = from y in con.Laboratory_Room
                      select y;
            ViewBag.LabList = lab;

            return View();
        }
        public ActionResult Manage()
        {
            return View();
        }

    }
}

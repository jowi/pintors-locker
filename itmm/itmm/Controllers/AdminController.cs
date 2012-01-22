using System;
using System.Collections.Generic;
using System.Web.Security;
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
        [Authorize(Roles = "Dev")]
        public ActionResult Index()
        {
            ViewBag.MyMessage = "Manage Laboratory";

            var room = from y in con.Rooms
                       select y;
            ViewBag.Room = room;

            var labroom = from y in con.Laboratory_Room
                          select y;
            ViewBag.LabRoom = labroom;

            var lablist = from y in con.Laboratories
                          orderby y.LaboratoryName ascending
                          select y;
            ViewBag.LabList = lablist;

            //LabHead


            return View();
        }
        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult Index(string LabName, string LabDesc, int[] room)
        {
            Laboratory f = new Laboratory();
            f.LaboratoryName = LabName;
            f.LaboratoryDesc = LabDesc;

            con.AddToLaboratories(f);
            con.SaveChanges();

            if (room != null)
            {

                foreach (var Room in room)
                {
                    Laboratory_Room a = new Laboratory_Room();
                    a.LaboratoryId = f.LaboratoryId;
                    var x = (from y in con.Rooms
                             where y.RoomId == Room
                             select y.RoomId).FirstOrDefault();
                    a.RoomId = x;

                    con.AddToLaboratory_Room(a);
                    con.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult EditLab(int? LabId)
        {
            var lablist = from y in con.Laboratories
                          select y;

            if (!LabId.Equals(null))
            {
                var x = from y in con.Laboratories
                        where y.LaboratoryId == LabId
                        select y;
                if (x != null)
                {
                    var labroom1 = from y in con.Laboratory_Room
                                   where y.LaboratoryId == LabId
                                   select y;
                    ViewBag.Room = labroom1;
                    var labroom2 = from y in con.Laboratory_Room
                                   select y;
                    var room = from y in con.Rooms
                               select y;
                    List<Room> AvailRoom = new List<Room>();
                    foreach (var _room in room)
                    {
                        int flag = 0;
                        foreach (var _labroom2 in labroom2)
                        {
                            if (flag == 0)
                            {
                                if (_room.RoomId == _labroom2.RoomId)
                                {
                                    flag = 1;
                                }
                            }
                        }
                        if (flag == 0)
                        {
                            AvailRoom.Add(_room);
                        }
                    }
                    ViewBag.AvailRoom = AvailRoom;

                    ViewBag.LabInfo = x;
                    return PartialView();
                }
            }
            return PartialView();
        }
        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult EditLab(int LabId, string LabName, string LabDesc, int[] room)
        {
            var x = (from y in con.Laboratories
                     where y.LaboratoryId == LabId
                     select y).FirstOrDefault();
            var z = from y in con.Laboratory_Room
                    where y.LaboratoryId == LabId
                    select y;
            x.LaboratoryName = LabName;
            x.LaboratoryDesc = LabDesc;


            foreach (var _z in z)
            {
                con.DeleteObject(_z);

            }
            try
            {
                foreach (var _room in room)
                {
                    Laboratory_Room a = new Laboratory_Room();

                    var c = (from y in con.Rooms
                             where y.RoomId == _room
                             select y.RoomId).FirstOrDefault();
                    a.RoomId = c;
                    a.LaboratoryId = LabId;

                    con.AddToLaboratory_Room(a);
                }
            }
            catch (Exception) { }
            con.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }
        [Authorize(Roles = "Dev")]
        public ActionResult DeleteLab(int LabId)
        {
                var x = (from y in con.Laboratories
                         where y.LaboratoryId == LabId
                         select y).FirstOrDefault();
                con.DeleteObject(x);
                con.SaveChanges();
                return RedirectToAction("Index", "Admin");      
            
        }
        [Authorize(Roles = "Dev")]
        public ActionResult Room()
        {
            var x = from y in con.Rooms
                    select y;
            ViewBag.RoomList = x;
            return View();
        }
        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult Room(String RoomName)
        {
                Room a = new Room();
                a.RoomName = RoomName;
                con.AddToRooms(a);
                con.SaveChanges();
                return RedirectToAction("Index", "Admin");
        }
        [Authorize(Roles = "Dev")]
        public ActionResult EditRoom(int RoomId)
        {
            try
            {
                var x = from y in con.Rooms
                        where y.RoomId == RoomId
                        select y;
                ViewBag.RoomInfo = x;
            }
            catch (Exception)
            {
      
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult EditRoom(int RoomId,string RoomName)
        {
            try
            {
                var x = (from y in con.Rooms
                        where y.RoomId == RoomId
                        select y).FirstOrDefault();
                x.RoomName = RoomName;
                con.SaveChanges();

            }
            catch (Exception)
            {
                
            }
            return RedirectToAction("Index", "Admin");
        }
        [Authorize(Roles = "Dev")]
        public ActionResult DeleteRoom(int RoomId)
        {
            try
            {
                var x = (from y in con.Rooms
                         where y.RoomId == RoomId
                         select y).FirstOrDefault();
                con.DeleteObject(x);
                con.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index", "Admin");
        }

       [Authorize(Roles = "Dev")]
        public ActionResult Head()
        {
            var x = from y in con.Laboratories
                    orderby y.LaboratoryName ascending
                    select y;
            ViewBag.LabList = x;
            var b = from y in con.Laboratory_Head
                    select y;
            ViewBag.HeadList = b;
            return PartialView();
        }
       [HttpPost]
       public ActionResult Head(itmmAdminHead model, int section)
       {
           var l = from y in con.Laboratories
                   orderby y.LaboratoryName ascending
                   select y;
           ViewBag.LabList = l;
           var b = from y in con.Laboratory_Head
                   select y;
           ViewBag.HeadList = b;

           if (ModelState.IsValid)
           {
               AccountMembershipService MembershipService = new AccountMembershipService();
               MembershipCreateStatus createStatus = MembershipService.CreateUser(model.uname, model.password, model.eadd);
               if (createStatus == MembershipCreateStatus.Success)
               {
                   Roles.AddUserToRole(model.uname, "Head");
                   Laboratory_Head a = new Laboratory_Head();
                   a.FirstName = model.fname;
                   a.LastName = model.lname;
                   a.UserName = model.uname;
                   a.ContactNum = model.cnum;
                   a.EmailAdd = model.eadd;
                   a.LaboratoryId = section;

                   var x = (from y in con.Laboratories
                           where y.LaboratoryId == section
                           select y).FirstOrDefault();

                   x.UserName = model.uname;

                   con.AddToLaboratory_Head(a);
                  
                   con.SaveChanges();


                   return RedirectToAction("Head", "Admin");
               }
               else
               {
                   ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
               }       
           }
           return View(model);
           
       }
        [Authorize(Roles = "Dev")]
       public ActionResult EditHead(int LabHeadId)
       {
           var x = from y in con.Laboratory_Head
                   where y.Laboratory_HeadId == LabHeadId
                   select y;
           ViewBag.LabHeadInfo = x;
           var a = from y in con.Laboratories
                   select y;
           ViewBag.LabList = a;
           return View();
       }
        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult EditHead(int LabHeadId,string fname,string lname,string cnum,string eadd,int section)
        {
            var x = (from y in con.Laboratory_Head
                     where y.Laboratory_HeadId == LabHeadId
                     select y).FirstOrDefault();
            x.FirstName = fname;
            x.LastName = lname;
            x.ContactNum = cnum;
            x.EmailAdd = eadd;
            var z = (from y in con.Laboratories
                     where y.LaboratoryId == section
                     select y).FirstOrDefault();
            z.UserName = x.UserName;
            con.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }
        [Authorize(Roles = "Dev")]
        public ActionResult DeleteHead(string UserName)
        {
            try
            {
                Membership.DeleteUser(UserName);
                //delete head on Laboratory
                var x = (from y in con.Laboratories
                         where y.UserName == UserName
                         select y).FirstOrDefault();
                x.UserName = null;
                //delete head on Laborator_Head
                var z = (from y in con.Laboratory_Head
                         where y.UserName == UserName
                         select y).FirstOrDefault();
               
                con.DeleteObject(z);
                con.SaveChanges();
            }
            catch (Exception)
            {
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }



    }
}

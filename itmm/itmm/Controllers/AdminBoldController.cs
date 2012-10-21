using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;

namespace itmm.Controllers
{
    public class AdminBoldController : Controller
    {
        public pintorEntities con = new pintorEntities();

        public ActionResult Index()
        {
            ViewBag.MyMessage = "Manage Laboratory";

            return View();
        }

        /* ROOM START */

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
        public ActionResult Room( itmmAdminRoom model )
        {

            Room a = new Room();
            a.RoomName = model.room;
            con.AddToRooms(a);
            con.SaveChanges();

            return RedirectToAction("Room", "AdminBold");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult EditRoom( int RoomId )
        {
            var x = (from y in con.Rooms
                     where y.RoomId == RoomId
                     select y).FirstOrDefault();

            itmmAdminRoom b = new itmmAdminRoom();

            b.room = x.RoomName;

            return View(b);
        }

        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult EditRoom( itmmAdminRoom model )
        {
            var x = (from y in con.Rooms
                     where y.RoomId == model.roomid
                     select y).FirstOrDefault();

            x.RoomName = model.room;

            con.SaveChanges();

            return RedirectToAction("Room", "AdminBold");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult DeleteRoom( int RoomId)
        {
            var x = (from y in con.Rooms
                     where y.RoomId == RoomId
                     select y).FirstOrDefault();

            con.DeleteObject(x);

            con.SaveChanges();

            return RedirectToAction("Room", "AdminBold");
        }

        public JsonResult IsRoomNameTaken(string room, int? roomid)
        {

            var x = (from y in con.Rooms
                     where y.RoomName == room
                     select y).FirstOrDefault();
            if (x == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // do not check for uniqueness when the object being edited owns the unique value
                if (x.RoomId == roomid && roomid != null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Format("{0} is already taken.", room), JsonRequestBehavior.AllowGet);
                }
                
            }

        }

        /* ROOM END */


        /* LABORATORY START */
        [Authorize(Roles = "Dev")]
        public ActionResult Laboratory()
        {

            var room = from y in con.Rooms
                       select y;
            ViewBag.Room = room;

            var labroom = from y in con.Laboratory_Room
                          select y;
            ViewBag.LabRoom = labroom;

            var lablist = from y in con.Laboratories
                          where y.inactive == 0
                          orderby y.LaboratoryName ascending
                          select y;
            ViewBag.LabList = lablist;

            return View();
        }

        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult Laboratory(itmmLaboratory model, int[] room)
        {

            Laboratory f = new Laboratory();
            f.LaboratoryName = model.labname;
            f.LaboratoryDesc = model.labdesc;
            f.LaboratoryContact = model.labcontact;
            f.inactive = 0;
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

            return RedirectToAction("Laboratory", "AdminBold");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult EditLaboratory(int LabId)
        {
            var x = (from y in con.Laboratories
                    where y.LaboratoryId == LabId
                    select y).FirstOrDefault();

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

            itmmLaboratory z = new itmmLaboratory();

            z.labid = x.LaboratoryId;
            z.labname = x.LaboratoryName;
            z.labdesc = x.LaboratoryDesc;
            z.labcontact = x.LaboratoryContact;

            return View(z);
        }

        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult EditLaboratory( itmmLaboratory model, int[] room )
        {
            // get lab object
            var x = (from y in con.Laboratories
                     where y.LaboratoryId == model.labid
                     select y).FirstOrDefault();

            // get all rooms linked to the lab
            var z = from y in con.Laboratory_Room
                    where y.LaboratoryId == model.labid
                    select y;

            x.LaboratoryName = model.labname;
            x.LaboratoryDesc = model.labdesc;
            x.LaboratoryContact = model.labcontact;

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
                    a.LaboratoryId = model.labid;

                    con.AddToLaboratory_Room(a);
                }
            }
            catch (Exception) { }

            con.SaveChanges();

            return RedirectToAction("Laboratory", "AdminBold");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult DeleteLaboratory(int LabId)
        {
            // just make lab inactive not delete it
            var x = (from y in con.Laboratories
                     where y.LaboratoryId == LabId
                     select y).FirstOrDefault();
            //con.DeleteObject(x);

            x.inactive = 1;

            // unlink rooms assigned to lab upon delete
            var z =  from y in con.Laboratory_Room
                     where y.LaboratoryId == LabId
                     select y;

            if (z != null)
            {
                foreach (var obj in z)
                {
                    con.DeleteObject( obj );
                }
            }

            // unlink head account assigned to lab upon delete
            var b = from y in con.Laboratory_Head
                    where y.LaboratoryId == LabId
                    select y;
            if (b != null)
            {
                foreach (var obj in b)
                {
                   // con.DeleteObject(obj);
                    
                }
            }



   
            con.SaveChanges();


            return RedirectToAction("Laboratory", "AdminBold");

        }

        //[Authorize(Roles = "Dev")]
        //public ActionResult RestoreLab(int LabId)
        //{
        //    var x = (from y in con.Laboratories
        //             where y.LaboratoryId == LabId
        //             select y).FirstOrDefault();
        //    x.inactive = 0;
        //    con.SaveChanges();

        //    return RedirectToAction("Index", "Admin");
        //}

        public JsonResult IsLabNameTaken(string labname)
        {

            var x = (from y in con.Laboratories
                     where y.LaboratoryName == labname
                     select y).FirstOrDefault();
            if (x == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(string.Format("{0} is already taken.", labname), JsonRequestBehavior.AllowGet);
            }

        }

        /* LABORATORY END */

        /* HEAD ACCOUNT START */
        [Authorize(Roles = "Dev")]
        public ActionResult Head()
        {
            var x = from y in con.Laboratories
                    where y.inactive == 0
                    orderby y.LaboratoryName ascending
                    select y;
            ViewBag.LabList = x;

            var b = from y in con.Laboratory_Head
                    select y;
            ViewBag.HeadList = b;

            return View();
        }

        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult Head( itmmAdminHead model, int section )
        {
            var r = from y in con.Laboratories
                    where y.inactive == 0
                    orderby y.LaboratoryName ascending
                    select y;
            ViewBag.LabList = r;

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
                    

                    con.AddToLaboratory_Head(a);
                   
                    var x = (from y in con.Laboratories
                             where y.LaboratoryId == section
                             select y).FirstOrDefault();


                    if (x == null) // when lab recors is not exist
                    {
                        x.UserName = model.uname;
                        x.DateUpdated = DateTime.Now;
                        con.AddToLaboratories(x);
                    }
                    else {
                        x.UserName = model.uname;
                        x.DateUpdated = DateTime.Now;
                    }
                    

                    con.SaveChanges();

                    return RedirectToAction("Head", "AdminBold");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Dev")]
        public ActionResult EditHead( int LabHeadId )
        {
            var x = (from y in con.Laboratory_Head
                    where y.Laboratory_HeadId == LabHeadId
                    select y).FirstOrDefault();

            ViewBag.LabHeadInfo = x.Laboratory_HeadId;


            itmmAdminHead a = new itmmAdminHead();

            a.fname = x.FirstName;
            a.lname = x.LastName;
            a.cnum = x.ContactNum;
            a.eadd = x.EmailAdd;

            // get all labs
            var b = from y in con.Laboratories
                    where y.inactive == 0
                    select y;

            ViewBag.LabList = b;
         

            return View(a);
        }

        [Authorize(Roles = "Dev")]
        [HttpPost]
        public ActionResult EditHead( itmmAdminHead model, int LabHeadId, int section )
        {
            var x = (from y in con.Laboratory_Head
                     where y.Laboratory_HeadId == LabHeadId
                     select y).FirstOrDefault();

            x.FirstName = model.fname;
            x.LastName = model.lname;
            x.ContactNum = model.cnum;
            x.EmailAdd = model.eadd;
            x.LaboratoryId = section;
            x.Laboratory.DateUpdated = DateTime.Now;

            // unassign head to its previous lab
            var b = from y in con.Laboratories
                     where y.UserName == x.UserName
                     select y;

                if( b != null ){
                    foreach (var item in b)
	                {
                        item.UserName = null;
	                }
                    
                }

            // assign head to lab
            var z = (from y in con.Laboratories
                     where y.LaboratoryId == section
                     select y).FirstOrDefault();

            z.UserName = x.UserName;

            con.SaveChanges();

            return RedirectToAction("Head", "AdminBold");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult DeleteHead(string UserName)
        {

            //delete head on Laboratory
            var x = (from y in con.Laboratories
                     where y.UserName == UserName
                     select y.UserName).FirstOrDefault();


            if (x != "")
            {

                x = null;

            }

            var z = (from y in con.Laboratory_Head
                     where y.UserName == UserName
                     select y).FirstOrDefault();

            con.DeleteObject(z);


            //delete User from aspnet users
            Membership.DeleteUser(UserName);


            con.SaveChanges();

            //    return View();
            return RedirectToAction("Head", "AdminBold");
        }

        /* HEAD ACCOUNT END */

    }
}

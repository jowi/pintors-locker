using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;

namespace itmm.Controllers
{
    public class StaffController : Controller
    {
        public pintorEntities con = new pintorEntities();
        public int getLabId()
        {
            var x = (from y in con.Laboratory_Staff
                     where y.UserName == User.Identity.Name
                     select y.LaboratoryId).FirstOrDefault();
            return x;
        }

        [Authorize(Roles="Staff")]
        public ActionResult Index()
        {
            return View();
        }
         [Authorize(Roles = "Staff")]
        public ActionResult Schedule()
        {
            
             int labid = getLabId();
             //room list  exclusive per lab
             var x = from y in con.Laboratory_Room
                     where y.LaboratoryId == labid orderby y.Room.RoomName                    
                     select y.Room;
             ViewBag.RoomList = x;
             //SkedList exclusive per lab
             var a = from y in con.Classes
                     where y.LabId == labid
                     select y;
             ViewBag.SkedList = a;
            return View();
        }
        [HttpPost]
         public ActionResult Schedule(itmmSchedule a, string room)
         {
             Class b = new Class();
             b.GroupNo = a.GroupNo;
             b.CourseCode = a.CourseCode;
             b.CourseDescription = a.CourseDesc;
             b.Day = a.Day;
             b.Schedule = a.Schedule;
             b.Instructor = a.Instructor;
             b.Room = room;

             b.LabId = getLabId();

             con.AddToClasses(b);
             con.SaveChanges();


             return RedirectToAction("Schedule");
         }
         [Authorize(Roles = "Staff")]
        public ActionResult EditSked(int SkedId)
        {
            try
            {
                //get class schedule
                var a = (from y in con.Classes
                         where y.ClassId == SkedId
                         select y).FirstOrDefault();

                itmmSchedule b = new itmmSchedule();

                b.GroupNo = a.GroupNo;
                b.CourseCode = a.CourseCode;
                b.CourseDesc = a.CourseDescription;
                b.Day = a.Day;
                b.Schedule = a.Schedule;
                b.Instructor = a.Instructor;

                int labid = getLabId();
                //room list  exclusive per lab
                var x = from y in con.Laboratory_Room
                        where y.LaboratoryId == labid
                        orderby y.Room.RoomName
                        select y.Room;
                ViewBag.RoomList = x;

                return View(b);
            }catch(Exception){
                return RedirectToAction("Schedule");
            }
            
        }
        [HttpPost]
         public ActionResult EditSked(itmmSchedule a,string room ,int SkedId)
         {
             var b = (from y in con.Classes
                     where y.ClassId == SkedId
                     select y).FirstOrDefault();

             b.GroupNo = a.GroupNo;
             b.CourseCode = a.CourseCode;
             b.CourseDescription = a.CourseDesc;
             b.Day = a.Day;
             b.Schedule = a.Schedule;
             b.Instructor = a.Instructor;
             b.Room = room;

             con.SaveChanges();
             return RedirectToAction("Schedule");
         }
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteSked(int SkedId)
        {
            try
            {
                var x = (from y in con.Classes
                         where y.ClassId == SkedId
                         select y).FirstOrDefault();
                con.DeleteObject(x);
                con.SaveChanges();
                return RedirectToAction("Schedule");
            }
            catch (Exception) {
                return RedirectToAction("Schedule");
            }
        }
        //REPORT
        [Authorize(Roles = "Staff")]
        public ActionResult Report()
        {
            ViewBag.Message = "Welcome to Report";
            return View();
        }

        [Authorize(Roles = "Staff")]
        public ActionResult Income()
        {
            return View();
        }

        [Authorize(Roles = "Staff")]
        public ActionResult Material()
        {
            var labid = getLabId();
            var x = from y in con.Laboratory_Material
                    where y.LaboratoryId == labid
                    select y.Material;
            ViewBag.MatList = x;

            return View();
        }


    }
}

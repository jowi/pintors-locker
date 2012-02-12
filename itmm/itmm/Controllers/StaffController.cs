using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;
using System.Text;

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
             b.AvailableTable = a.AvailableTable;
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
                b.AvailableTable = Convert.ToInt32(a.AvailableTable);

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
             b.AvailableTable = a.AvailableTable;

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

        //CLASS STUDENTS 
        [Authorize(Roles = "Staff")]
        public ActionResult ClassStudents(int skedId)
        {
            var x = from y in con.Classes
                    where y.ClassId == skedId
                    select y;
            ViewBag.Message = "Welcome to students";
            return View();
        }

        //REPORT
        [Authorize(Roles = "Staff")]
        public ActionResult Report()
        {
            ViewBag.Message = "Welcome to Report";
            return View();
        }

        [Authorize(Roles = "Staff")]
        public ActionResult Liability()
        {
            var labid = getLabId();
            var x = from y in con.Liabilities
                    where y.LaboratoryId == labid
                    select y;
            ViewBag.LList = x;

            return View();
        }
        [HttpPost]
        public ActionResult Liability(itmmLiability a)
        {

            StudentInfo c = new StudentInfo();
            c.FamiliyName = a.FamilyName;
            c.FirstName = a.FirstName;
            c.IdNumber = a.IdNumber;
            c.CourseAndYear = a.Course;
            con.AddToStudentInfoes(c);


            var labid = getLabId();
            Liability b = new Liability();
            b.Equipment = a.Equipment;
            b.Fine = a.Fine;
            b.Status = a.Status;
            b.LaboratoryId = labid;
            b.StudentId = c.StudentId;
            con.AddToLiabilities(b);

            con.SaveChanges();


            return RedirectToAction("Liability");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult EditLiability(int LiabilityId)
        {
            var c = (from y in con.Liabilities
                     where y.LiabilityId == LiabilityId
                     select y).FirstOrDefault();

            var b = (from y in con.Liabilities
                     where y.LiabilityId == LiabilityId
                     select y.StudentInfo).FirstOrDefault();

            itmmLiability a = new itmmLiability();
            a.FamilyName = b.FamiliyName;
            a.FirstName=b.FirstName;
            a.IdNumber=b.IdNumber;
            a.Course=b.CourseAndYear;

            a.Equipment = c.Equipment;
            a.Fine = c.Fine;
            a.Status = c.Status;

            return View(a);
        }
    [HttpPost]
        public ActionResult EditLiability(int LiabilityId, itmmLiability a)
        {
            var c = (from y in con.Liabilities
                     where y.LiabilityId == LiabilityId
                     select y).FirstOrDefault();

            var b = (from y in con.Liabilities
                     where y.LiabilityId == LiabilityId
                     select y.StudentInfo).FirstOrDefault();

            b.FamiliyName = a.FamilyName;
            b.FirstName = a.FirstName;
            b.IdNumber = a.IdNumber;
            b.CourseAndYear = a.Course;


            c.Equipment = a.Equipment;
            c.Fine = a.Fine;
            c.Status = a.Status;

            con.SaveChanges();

            return RedirectToAction("Liability");
        }

        [Authorize(Roles = "Staff")]
    public ActionResult Income()
    {
            // TODO datatables 

        var x = from y in con.Incomes
                select y;
        ViewBag.IncomeList = x;

        return View();
    }
        [HttpPost]
        public ActionResult Income(itmmIncome a)
        
        {
            StudentInfo c = new StudentInfo();
            c.FirstName = a.FirstName;
            c.FamiliyName = a.FamilyName;
            c.IdNumber = a.IdNumber;
            c.CourseAndYear = a.Course;
            con.AddToStudentInfoes(c);

            itmm.Models.Income b = new itmm.Models.Income();
            b.Transactionn = a.Transaction;
            b.cost = a.Cost;
            b.StudentId = c.StudentId;
            b.LaboratoryId = getLabId();
            con.AddToIncomes(b);

            con.SaveChanges();
            return RedirectToAction("Income");
        }

        public ActionResult EditIncome(int IncomeId)
        {
            var x = (from y in con.Incomes
                    where y.IncomeId == IncomeId
                    select y).FirstOrDefault();

            itmmIncome a = new itmmIncome();
            a.FamilyName = x.StudentInfo.FamiliyName;
            a.FirstName = x.StudentInfo.FirstName;
            a.IdNumber = x.StudentInfo.IdNumber;
            a.Course = x.StudentInfo.CourseAndYear;
            a.Cost =Convert.ToInt32(x.cost);
            a.Transaction = x.Transactionn;
            return View(a);
        }

        [HttpPost]
        public ActionResult EditIncome(itmmIncome a, int IncomeId)
        {

           

            var x = (from y in con.Incomes
                     where y.IncomeId == IncomeId
                     select y).FirstOrDefault();
            x.StudentInfo.FirstName = a.FirstName;
            x.StudentInfo.FamiliyName = a.FamilyName;
            x.StudentInfo.IdNumber = a.IdNumber;
            x.StudentInfo.CourseAndYear = a.Course;
            x.cost = a.Cost;
            x.Transactionn = a.Transaction;
            con.SaveChanges();

            return RedirectToAction("Income");
        }

         [Authorize(Roles = "Staff")]
        public ActionResult InventoryCost()
        {
            var labid = getLabId();
            var x = from y in con.Laboratory_Material
                    where y.LaboratoryId == labid
                    select y.Material;
            ViewBag.MatList = x;

            return View();
        }
        [Authorize(Roles = "Staff")]
         public ActionResult EditMaterial(int MatId)
         {
             var x = (from y in con.Materials
                      where y.MaterialId == MatId
                      select y).FirstOrDefault();

             itmmMaterial a = new itmmMaterial();
             a.Name = x.Name;
             a.Description = x.Description;
             a.Quantity = x.Quantity;
             return View(a);
         }
       [HttpPost]
        public ActionResult EditMaterial(int MatId, itmmMaterial a)
        {
            var b = (from y in con.Materials
                     where y.MaterialId == MatId
                     select y).FirstOrDefault();
           

            b.Name = a.Name;
            b.Description = a.Description;
            b.Quantity = a.Quantity;
            b.DateUpdated = DateTime.Now;
            con.SaveChanges();
            return RedirectToAction("InventoryCost");
        }
        [Authorize(Roles = "Staff")]
       public ActionResult DeleteMaterial(int MatId)
       {
           var x = (from y in con.Materials
                    where y.MaterialId == MatId
                    select y).FirstOrDefault();
           con.DeleteObject(x);

           var a = (from y in con.Laboratory_Material
                    where y.MaterialId == MatId
                    select y).FirstOrDefault();
           con.DeleteObject(a);

           con.SaveChanges();
           return RedirectToAction("InventoryCost");
       }

    }
}

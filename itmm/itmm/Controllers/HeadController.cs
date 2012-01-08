using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using itmm.Models;


namespace itmm.Controllers
{
    public class HeadController : Controller
    {
        public pintorEntities con = new pintorEntities();

        public bool IsStillHead()
        {
            var x = (from y in con.Laboratories
                     where y.UserName == User.Identity.Name
                     select y).FirstOrDefault();
            if (x == null)
            {
                return true;
            }
            else {
                return false;
            }
        }
        public int getLabId()
        {
            var x = (from y in con.Laboratories
                     where y.UserName == User.Identity.Name
                     select y).FirstOrDefault();
            return x.LaboratoryId;
        }
       
        //
        // GET: /Head/
    [Authorize(Roles="Head")]
        public ActionResult Index()
        {

            if (IsStillHead())
            {
                return RedirectToAction("Error");
            }
            else {
                var x = (from y in con.Laboratories
                         where y.UserName == User.Identity.Name
                         select y).FirstOrDefault();
                ViewBag.Welcome = "Welcome to" + " " + x.LaboratoryName;
            }

           
            return View();
        }
    [Authorize(Roles = "Head")]
    public ActionResult Error()
    {
        return View();
    }
    [Authorize(Roles = "Head")]
    public ActionResult Staff()
    {
        if (IsStillHead())
        {
            return RedirectToAction("Error");
        }

        var x = (from y in con.Laboratories
                 where y.UserName == User.Identity.Name
                 select y).FirstOrDefault();
        if (x == null)
        {
            return RedirectToAction("Error");
        }
        else
        {
            var z = from y in con.Laboratory_Staff
                    where y.LaboratoryId == x.LaboratoryId
                    select y;
            ViewBag.StaffList = z;
        }
          
        return View();
    }
    [Authorize(Roles = "Head")]
    [HttpPost]
    public ActionResult Staff(itmmAdminStaff model, string type)
    {
        if (ModelState.IsValid) {
            AccountMembershipService MembershipService = new AccountMembershipService();
            MembershipCreateStatus createStatus = MembershipService.CreateUser(model.uname, model.password, model.eadd);
            if (createStatus == MembershipCreateStatus.Success) {

                Roles.AddUserToRole(model.uname, "Staff");

                Laboratory_Staff a = new Laboratory_Staff();
                a.FirstName = model.fname;
                a.LastName = model.lname;
                a.IdNumber = model.cnum;
                a.CourseAndYear = model.course;
                a.EmailAddress = model.eadd;
                a.Type = type;
                a.UserName = model.uname;
                //for LabId
                var c = (from y in con.Laboratories
                         where y.UserName == User.Identity.Name
                         select y.LaboratoryId).FirstOrDefault();
                a.LaboratoryId = c;
                con.AddToLaboratory_Staff(a);
                con.SaveChanges();

                return RedirectToAction("Staff", "Head");
            }
            else
            {
                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            }       
        }
        return View(model);
    }
    [Authorize(Roles = "Head")]
    public ActionResult EditStaff(string UserName)
    {
        if (IsStillHead())
        {
            return RedirectToAction("Error");
        }
        var z = from y in con.Laboratory_Staff
              where y.UserName == UserName
               select y;
        itmmAdminStaff model = new itmmAdminStaff();
        foreach (var _z in z)
        {
            model.fname = _z.UserName;
            model.lname = _z.LastName;
            model.cnum = _z.IdNumber;
            model.course = _z.CourseAndYear;
            model.eadd = _z.EmailAddress;
            model.uname = _z.UserName;
        }
        
        return View(model);
    }
    [Authorize(Roles = "Head")]
    [HttpPost]
    public ActionResult EditStaff(itmmAdminStaff model)
    {
        var x = (from y in con.Laboratory_Staff
                 where y.UserName == model.uname
                 select y).FirstOrDefault();
       x.FirstName = model.fname;
       x.LastName = model.lname;
       x.IdNumber = model.cnum;
       x.CourseAndYear = model.course;
       x.EmailAddress = model.eadd;
       con.SaveChanges();
       return RedirectToAction("Staff", "Head");
    }
    [Authorize(Roles = "Head")]
    public ActionResult DeleteStaff(string UserName)
    {
        if (IsStillHead())
        {
            return RedirectToAction("Error");
        }
        Membership.DeleteUser(UserName);
        var x = (from y in con.Laboratory_Staff
                 where y.UserName == UserName
                 select y).FirstOrDefault();
        con.DeleteObject(x);
        con.SaveChanges();
        return RedirectToAction("Staff", "Head");
    }
    [Authorize(Roles = "Head")]
    public ActionResult Equipment()
    {
        if (IsStillHead())
        {
            return RedirectToAction("Error");
        }
        var x = from y in con.Equipments
                select y;
        ViewBag.EquipList = x;
        return View();
    }
    [Authorize(Roles = "Head")]
    [HttpPost]
    public ActionResult Equipment(itmmAdminEquipment model)
    {
        Equipment e = new Equipment();
        e.Make = model.make;
        e.Description = model.description;
        e.Location = model.location;
        e.SerialNumber = model.serial;
        e.Barcode = model.barcode;
        e.Status = model.status;
        e.LifeExpectancy = model.life;
        e.DateReceived = DateTime.Now;

        con.AddToEquipments(e);

        //Laboratory_Equipment
        Laboratory_Equipment le = new Laboratory_Equipment();
        le.EquipmentId = e.EquipmentId;
        le.LaboratoryId = getLabId();
        con.AddToLaboratory_Equipment(le);

        con.SaveChanges();

        return RedirectToAction("Equipment", "Head");
    }
   [Authorize(Roles = "Head")]
    public ActionResult EditEquipment(int EquipId)
    {
        if (IsStillHead())
        {
            return RedirectToAction("Error");
        }

        var x = (from y in con.Equipments
                where y.EquipmentId == EquipId
                select y).FirstOrDefault();
        itmmAdminEquipment model = new itmmAdminEquipment();
            model.make = x.Make;
            model.description = x.Description;
            model.location = x.Location;
            model.serial = x.SerialNumber;
            model.status = x.Status;
            model.life = x.LifeExpectancy;

        return View(model);
    }
   [Authorize(Roles = "Head")]
   [HttpPost]
   public ActionResult EditEquipment(itmmAdminEquipment model, int EquipId)
   {
       var x = (from y in con.Equipments
                where y.EquipmentId == EquipId
                select y).FirstOrDefault();
      x.Make = model.make;
      x.Description = model.description;
      x.Location= model.location;
      x.SerialNumber = model.serial;
      x.Status = model.status;
      x.LifeExpectancy = model.life;
      con.SaveChanges();
      return RedirectToAction("Equipment", "Head");
   }
  [Authorize(Roles = "Head")]
   public ActionResult DeleteEquipment(int EquipId)
   {
       if (IsStillHead())
       {
           return RedirectToAction("Error");
       }
      //Laboratory_Equipment
       var x = (from y in con.Laboratory_Equipment
                where y.EquipmentId == EquipId
                select y).FirstOrDefault();
       con.DeleteObject(x);
       var z = (from y in con.Equipments
                where y.EquipmentId == EquipId
                select y).FirstOrDefault();
       con.DeleteObject(z);
       con.SaveChanges();
       return RedirectToAction("Equipment", "Head");
   }
  [Authorize(Roles = "Head")]
  public ActionResult Notification()
  {
      if (IsStillHead())
      {
          return RedirectToAction("Error");
      }
      int labid = getLabId();
      //for dataTables
      var x = from y in con.Notifications
              where y.LaboratoryId == labid
              select y;
      ViewBag.NoticeList = x;
      //for active notification
      var time = DateTime.Now.ToString("MM/dd/yyyy");
      var a = from y in con.Notifications
              where y.Whin == time
              select y;
      ViewBag.Notice = a;
      return View();
  }

  [HttpPost]
  public ActionResult Notification(itmmNotification model)
  {

      Notification a = new Notification();
      a.LaboratoryId = getLabId();
      a.What = model.what;
      a.Whin = model.whin;
      a.Whire = model.whire;
      a.Who = model.who;
      a.Time = model.time;

      con.AddToNotifications(a);
      con.SaveChanges();
      return RedirectToAction("Notification", "Head");
  }
  [Authorize(Roles = "Head")]
  public ActionResult EditNotification(int NoticeId)
  {
      if (IsStillHead())
      {
          return RedirectToAction("Error");
      }
      var x = (from y in con.Notifications
               where y.NotificationId == NoticeId
               select y).FirstOrDefault();
      itmmNotification a = new itmmNotification();
      a.what = x.What;
      a.whin = x.Whin;
      a.whire = x.Whire;
      a.who = x.Who;
      a.time = x.Time;
     
      return View(a);
  }
  [Authorize(Roles = "Head")]
  [HttpPost]
  public ActionResult EditNotification(itmmNotification model, int NoticeId)
  {
      var a = (from y in con.Notifications
               where y.NotificationId == NoticeId
               select y).FirstOrDefault();
      a.LaboratoryId = getLabId();
      a.What = model.what;
      a.Whin = model.whin;
      a.Whire = model.whire;
      a.Who = model.who;
      a.Time = model.time;

      con.SaveChanges();
      return RedirectToAction("Notification", "Head");
  }
  [Authorize(Roles = "Head")]
  public ActionResult DeleteNotification(int NoticeId)
  {
      var x = (from y in con.Notifications
              where y.NotificationId == NoticeId
              select y).FirstOrDefault();
      con.DeleteObject(x);
      con.SaveChanges();

      return RedirectToAction("Notification", "Head");
  }
  public ActionResult grit()
  {
      return View();
  }

    }
}

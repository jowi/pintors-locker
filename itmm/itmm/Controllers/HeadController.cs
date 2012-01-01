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
        //
        // GET: /Head/
    [Authorize(Roles="Head")]
        public ActionResult Index()
        {
            var x = (from y in con.Laboratories
                     where y.UserName == User.Identity.Name
                     select y).FirstOrDefault();
            if (x == null)
            {
                return RedirectToAction("Error");
            }
            else {
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
        Membership.DeleteUser(UserName);
        var x = (from y in con.Laboratory_Staff
                 where y.UserName == UserName
                 select y).FirstOrDefault();
        con.DeleteObject(x);
        con.SaveChanges();
        return RedirectToAction("Staff", "Head");
    }
       

    }
}

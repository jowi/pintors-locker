﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using itmm.Models;

namespace itmm.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public pintorEntities con = new pintorEntities();

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public bool IsLabInactiveHead(string uname)
        {

            var x = (from y in con.Laboratories
                    where y.UserName == uname
                    orderby y.DateUpdated descending
                    select y.inactive ).FirstOrDefault();

            if (x != 0)//0 means, lab is active 
            {
                return true; 
            }
            else{
                return false;
            }
        }

        public bool IsLabInactiveStaff(string uname)
        {

            var x = (from y in con.Laboratory_Staff
                     where y.UserName == uname
                     select y.Laboratory.inactive).FirstOrDefault();

            if (x != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            if (User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (Roles.IsUserInRole(model.UserName, "Dev"))
                        {
                            return RedirectToAction("Index", "AdminBold");
                        }
                        else if(Roles.IsUserInRole(model.UserName,"Head")){

                            if( IsLabInactiveHead(model.UserName) )
                            {
                                return RedirectToAction("InactiveLab", "Error");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Head");
                            }
                        }
                        else if(Roles.IsUserInRole(model.UserName,"Staff")){

                            if (IsLabInactiveStaff(model.UserName))
                            {
                                return RedirectToAction("InactiveLab", "Error");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Staff");
                            }
                            
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {

            return View();
        }



        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(itmm.Models.itmmForgotPassword model)
        {

            var a = (from y in con.aspnet_Users
                     where y.UserName == model.uname
                     select y.UserId).FirstOrDefault();

            var b = (from y in con.aspnet_Membership
                     where y.UserId == a
                     select y.Password).FirstOrDefault();

            MembershipUser currentUser = Membership.GetUser(model.uname);
            currentUser.UnlockUser();
            string password = currentUser.ResetPassword();


            if (MembershipService.ChangePassword(model.uname, password, "Pass12345"))
            {
                return RedirectToAction("ChangePasswordSuccess");
            }
            else
            {
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            return View();
        }


    }
}

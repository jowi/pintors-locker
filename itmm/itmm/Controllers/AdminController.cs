﻿using System;
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

            var labroom = from y in con.Laboratory_Room
                      select y;
            ViewBag.LabRoom = labroom;

            var lablist = from y in con.Laboratories
                          orderby y.LaboratoryName ascending
                          select y;
            ViewBag.LabList = lablist;
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
            return RedirectToAction("Index","Admin");
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
            }catch(Exception){}
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
 

    }
}

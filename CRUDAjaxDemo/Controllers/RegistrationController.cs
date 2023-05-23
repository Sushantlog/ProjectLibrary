﻿using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAjaxDemo.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            tbl_Registration tblReg = new tbl_Registration();
            return View(tblReg);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(RegisterViewModel objRegister)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            // Validation
            //check username/email/password are unique if duplicate found send error

            tbl_Registration tblRegistration = new tbl_Registration();
            tblRegistration.UserName = objRegister.UserName;
            tblRegistration.Email = objRegister.Email;
            tblRegistration.Password = objRegister.Password;

            std.tbl_Registration.Add(tblRegistration);
            std.SaveChanges();

            var redirectToUrl = Url.Action("Index", "Login");

            return Json(new
            {
                IsValid = true,
                ResultMessage = "Registered Successfully",
                Id = tblRegistration.UserID,
                RedirectToUrl = redirectToUrl
            }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult UserProfile(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            var UserDetails = std.tbl_Registration.Where(m => m.UserID == Id).Select(m => new ProfileViewModel
            {
                UserId = m.UserID,
                UserName = m.UserName,
                Email = m.Email,
            }).FirstOrDefault();
            return View(UserDetails);
        }

        [HttpGet]
        public ActionResult EditUserProfile(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            var UserDetails = std.tbl_Registration.Where(m => m.UserID == Id).Select(m => new ProfileViewModel
            {
                UserId = m.UserID,
                UserName = m.UserName,
                Email = m.Email,
            }).FirstOrDefault();
            return View(UserDetails);
        }
    }
}
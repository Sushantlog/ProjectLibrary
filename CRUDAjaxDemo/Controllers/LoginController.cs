﻿using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace CRUDAjaxDemo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            LoginViewModel objLogin = new LoginViewModel();

            return View(objLogin);
        }

        // post: Login
        public JsonResult Userlogin(LoginViewModel objLogin)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                std.Database.Connection.Open();
                var resultMessage = string.Empty;
                var isValid = false;
                var id = 0;
                var IsvalidUser = false;
                if (std.tbl_Registration.Any(m => m.Email == objLogin.Email))
                {
                    IsvalidUser = true;
                }
                if (IsvalidUser)
                {
                    var UserDetails = std.tbl_Registration.Where(m => m.Email == objLogin.Email).FirstOrDefault();
                    if (UserDetails.Password == objLogin.Password)
                    {
                        isValid = true;
                        resultMessage = "Login Successfull";
                        Session["UserID"] = UserDetails.UserID.ToString();
                        id = UserDetails.UserID;
                    }
                    else
                    {
                        isValid = false;
                        resultMessage = "Invalid Password";
                    }
                    return Json(
                        new
                        {
                            IsValid = isValid,
                            ResultMessage = resultMessage,
                            Id = id,
                            redirectToUrl = Url.Action("Index", "Home", new { Id = id })
                        }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    isValid = false;
                    resultMessage = "Invalid Email";
                }
                return Json(new { IsValid = isValid, ResultMessage = resultMessage, Id = id }, JsonRequestBehavior.AllowGet);
            }
            ;

        }

        // GET: 
        public ActionResult ForgotPassword()
        {
            LoginViewModel objLogin = new LoginViewModel();
            return View(objLogin);
        }

        // GET: 
        public ActionResult SendOTP(LoginViewModel objLogin)
        {
            try
            {
                using (ProjectLibraryEntities std = new ProjectLibraryEntities())
                {
                    var UserDetails = std.tbl_Registration.Where(m => m.Email == objLogin.Email).FirstOrDefault();
                    var OTP = GenerateRandomNo().ToString();
                    if (UserDetails != null)
                    {
                        tbl_OTP tbl_OTP = new tbl_OTP();
                        tbl_OTP.UserID = UserDetails.UserID;
                        tbl_OTP.OTP = OTP;
                        std.tbl_OTP.Add(tbl_OTP);
                        std.SaveChanges();
                    }
                    try
                    {
                        string username = "TestAppSushant2023@Gmail.com";
                        string password = "hfjejdequdchldex";
                        ICredentialsByHost credentials = new NetworkCredential(username, password);

                        SmtpClient smtpClient = new SmtpClient()
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            Credentials = credentials
                        };

                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(username);
                        mail.To.Add(objLogin.Email);
                        mail.Subject = "OTP";
                        string Body = OTP;
                        mail.Body = Body;

                        smtpClient.Send(mail);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    return Json(new { IsValid = true, ResultMessage = "Send OTP on email " + objLogin.Email }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsValid = false, ResultMessage = "Error - " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Generate RandomNo
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        // post: Login
        public JsonResult ValidateOTP(LoginViewModel objOTP)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var resultMessage = string.Empty;
                var isValid = false;
                var id = 0;

                if (std.tbl_Registration.Any(m => m.Email == objOTP.Email))
                {
                    var UserDetails = std.tbl_Registration.Where(m => m.Email == objOTP.Email).FirstOrDefault();
                    var OTPDetails = std.tbl_OTP.Where(m => m.UserID == UserDetails.UserID).OrderByDescending(m => m.OTPID).FirstOrDefault();
                    if (OTPDetails != null && OTPDetails.OTP == objOTP.OTP)
                    {
                        isValid = true;
                        resultMessage = "Correct OTP";
                        id = (int)(OTPDetails?.UserID);
                        return Json(
                        new
                        {
                            IsValid = isValid,
                            ResultMessage = resultMessage,
                            Id = id,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        isValid = false;
                        resultMessage = "Invalid OTP";
                    }
                }
                else
                {
                    isValid = false;
                    resultMessage = "Invalid Email";
                }
                return Json(new { IsValid = isValid, ResultMessage = resultMessage, Id = id }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangePasswordPage(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            LoginViewModel objlogin = new LoginViewModel();
            objlogin.UserId = Id;
            return View(objlogin);

        }
        public JsonResult ChangePassword(LoginViewModel objChangePassword)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var resultMessage = string.Empty;
                var isValid = false;
                var id = 0;
                tbl_Registration UserDetails = null;
                if (objChangePassword.UserId > 0)
                {
                    UserDetails = std.tbl_Registration.Where(m => m.UserID == objChangePassword.UserId).FirstOrDefault();
                }
                else
                {
                    UserDetails = std.tbl_Registration.Where(m => m.Email == objChangePassword.Email).FirstOrDefault();
                }
                if (UserDetails != null)
                {
                    UserDetails.Password = objChangePassword.Password;
                    std.SaveChanges();
                    try
                    {
                        string username = "TestAppSushant2023@Gmail.com";
                        string password = "hfjejdequdchldex";
                        ICredentialsByHost credentials = new NetworkCredential(username, password);

                        SmtpClient smtpClient = new SmtpClient()
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            Credentials = credentials
                        };

                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(username);
                        mail.To.Add(UserDetails.Email);
                        mail.Subject = "Password Changed for Project Library";
                        string Body = "Password changed for user Id : " + objChangePassword.Email + "\nNew Password is: " + UserDetails.Password;
                        mail.Body = Body;

                        smtpClient.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    isValid = true;
                    resultMessage = "password changed successfully";
                    id = (int)(UserDetails?.UserID);
                    return Json(
                    new
                    {
                        IsValid = true,
                        ResultMessage = resultMessage,
                        Id = id,
                        RedirectToUrl = Url.Action("Index", "Login")
                    });
                }
                else
                {
                    isValid = false;
                    resultMessage = "Invalid User";
                }
                return Json(new { IsValid = isValid, ResultMessage = resultMessage, Id = id }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}
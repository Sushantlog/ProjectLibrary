using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;
using CRUDAjaxDemo.Models;

namespace CRUDAjaxDemo.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ContactViewModel objContact = new ContactViewModel();
            objContact.LoginUserId = Id;
            objContact.SupportEmail = "TestAppSushant2023@Gmail.com";
            objContact.ContactList = new List<ContactDetailsViewModel>();

            ContactDetailsViewModel ContactPerson = new ContactDetailsViewModel();
            ContactPerson.Name = "Sushant Balu Patil";
            ContactPerson.Email = "sushantbpatil53@gmail.com";
            ContactPerson.MobileNumber = "7887477459";
            objContact.ContactList.Add(ContactPerson);

            ContactPerson = new ContactDetailsViewModel();
            ContactPerson.Name = "Aniket Avinash Patil";
            ContactPerson.Email = "avinashpatil98672@gmail.com";
            ContactPerson.MobileNumber = "9096230241";
            objContact.ContactList.Add(ContactPerson);

            ContactPerson = new ContactDetailsViewModel();
            ContactPerson.Name = "Suraj Rajaram Patil";
            ContactPerson.Email = "ravi988115@gmail.com";
            ContactPerson.MobileNumber = "9881156616";
            objContact.ContactList.Add(ContactPerson);

            ContactPerson = new ContactDetailsViewModel();
            ContactPerson.Name = "Abhishek Rajaram Sutar";
            ContactPerson.Email = "abhisutar5548@gmail.com";
            ContactPerson.MobileNumber = "8007721797";
            objContact.ContactList.Add(ContactPerson);

            return View(objContact);
        }

        public ActionResult SendMail(MailModel ObjMail)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var LoggedInUserDetails = std.tbl_Registration.Where(m => m.UserID == ObjMail.UserId).FirstOrDefault();
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
                    mail.To.Add(username);
                    mail.Subject = "Contacted By " + LoggedInUserDetails.UserName + "[" + LoggedInUserDetails.Email + "]";
                    string Body = ObjMail.Message;
                    mail.Body = Body;

                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                return Json(new { IsValid = true, ResultMessage = "Your Message has been sent to admin." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
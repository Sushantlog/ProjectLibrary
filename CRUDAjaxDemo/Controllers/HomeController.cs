using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAjaxDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();

            var UserDetails = std.tbl_Registration.Where(m => m.UserID == Id).FirstOrDefault();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Email = UserDetails.Email;
            homeViewModel.UserId = UserDetails.UserID;
            homeViewModel.UserName = UserDetails.UserName;

            CounterModel objCounter = new CounterModel();
            objCounter.TotalUsers = std.tbl_Registration.Count();
            objCounter.TotalFiles = std.tbl_FilesDetails.Count();
            objCounter.TotalProjects = std.tbl_SynopsisDetails.Count();
            objCounter.TotalActiveUsers = std.tbl_SynopsisDetails.Select(m => m.UserID).Distinct().Count();
            homeViewModel.counter = objCounter;
            return View(homeViewModel);
        }
    }
}
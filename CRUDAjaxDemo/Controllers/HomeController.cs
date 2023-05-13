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
            ProjectLibraryEntities std = new ProjectLibraryEntities();

            var UserDetails = std.tbl_Registration.Where(m => m.UserID == Id).FirstOrDefault();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Email = UserDetails.Email;
            homeViewModel.UserId = UserDetails.UserID;

            return View(homeViewModel);
        }
    }
}
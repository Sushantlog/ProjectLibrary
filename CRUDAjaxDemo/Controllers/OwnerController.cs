using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAjaxDemo.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        //POST: Default
        [HttpPost]
        public ActionResult SaveOwner(owner_login Owner)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            std.owner_login.Add(Owner);
            std.SaveChanges();
            return Json(new { IsValid = true, ResultMessage = "Success"}, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ShowOwner()
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            var Obj=  std.owner_login.Where(m => m.Ownerid == 1).FirstOrDefault();
            return View("Index",Obj);

        }
    }
}
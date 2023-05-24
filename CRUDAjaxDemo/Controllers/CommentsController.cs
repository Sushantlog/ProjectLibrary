using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAjaxDemo.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        public ActionResult Index(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                CommentsViewModel comments = new CommentsViewModel();
                comments.UserId = Id;
                return View(comments);
            }
        }

        // POST: Comments
        public ActionResult SaveComments(CommentsViewModel objComment)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                tbl_Comments tblComments = new tbl_Comments();
                tblComments.UserID = objComment.UserId;
                tblComments.CommentHeader = objComment.CommentHeader;
                tblComments.CommentDescription = objComment.CommentDescription;
                std.tbl_Comments.Add(tblComments);
                std.SaveChanges();

                return Json(new
                {
                    IsValid = true,
                    ResultMessage = "Comment saved Successfully",
                    Id = tblComments.CommentId,
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
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
            if (Session["UserID"] != null && Session["UserID"].ToString() != objComment.UserId.ToString())
            {
                return RedirectToAction("Index", "login");
            }

            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var IsValid = false;
                var ResultMessage = string.Empty;
                var Id = 0;
                if (!objComment.IsEdit)
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
                else
                {
                    tbl_Comments tblComments = std.tbl_Comments.Where(m => m.CommentId == objComment.CommentId && m.UserID == objComment.UserId).FirstOrDefault();
                    if (tblComments != null)
                    {
                        tblComments.CommentHeader = objComment.CommentHeader;
                        tblComments.CommentDescription = objComment.CommentDescription;
                        std.SaveChanges();
                        IsValid = true;
                        ResultMessage = "Comment saved Successfully";
                        Id = tblComments.CommentId;
                    }
                    else
                    {
                        IsValid = false;
                        ResultMessage = "Record not found.";
                    }

                    return Json(new
                    {
                        IsValid = IsValid,
                        ResultMessage = ResultMessage,
                        Id = Id,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ShowComments(int Id)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var comments = std.tbl_Comments.Where(m => m.UserID == Id).ToList();
                var ShowCommentsViewModel = new ShowCommentsViewModel();
                ShowCommentsViewModel.LoginUserId = Id;
                ShowCommentsViewModel.CommentList = new List<CommentsViewModel>();
                for (int i = 0; i < comments.Count; i++)
                {
                    CommentsViewModel objComment = new CommentsViewModel();
                    objComment.Index = i + 1;
                    objComment.UserId = (int)comments[i].UserID;
                    objComment.CommentId = comments[i].CommentId;
                    objComment.CommentHeader = comments[i].CommentHeader;
                    objComment.CommentDescription = comments[i].CommentDescription;
                    ShowCommentsViewModel.CommentList.Add(objComment);
                }
                return View(ShowCommentsViewModel);
            }
        }

        public ActionResult EditComment(int userId, int CommentId)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var commentDetails = std.tbl_Comments.Where(m => m.CommentId == CommentId && m.UserID == userId).FirstOrDefault();
                CommentsViewModel objComment = new CommentsViewModel();
                if (commentDetails != null)
                {
                    objComment.CommentId = commentDetails.CommentId;
                    objComment.UserId = (int)commentDetails.UserID;
                    objComment.CommentHeader = commentDetails.CommentHeader;
                    objComment.CommentDescription = commentDetails.CommentDescription;
                }
                return View("Index", objComment);
            }
        }


        public ActionResult DeleteComment(int CommentId)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var IsValid = false;
                var ResultMessage = "";
                var CommentDetails = std.tbl_Comments.Where(m => m.CommentId == CommentId).FirstOrDefault();
                if (CommentDetails != null)
                {
                    std.tbl_Comments.Remove(CommentDetails);
                    std.SaveChanges();

                    IsValid = true;
                    ResultMessage = "Comment Deleted successfully!";
                }
                else
                {
                    IsValid = false;
                    ResultMessage = "Record not exist!";
                }
                return Json(new
                {
                    IsValid = IsValid,
                    ResultMessage = ResultMessage
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
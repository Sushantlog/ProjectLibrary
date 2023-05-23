using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebSockets;
using PagedList;
using PagedList.Mvc;

namespace CRUDAjaxDemo.Controllers
{
    public class SynopsisController : Controller
    {
        // GET: Synopsis
        public ActionResult Index(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();

            var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
            ViewData["Category"] = categoryMaster;

            var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
            ViewData["College"] = collegeMaster;

            SynopsisModel synopsisModel = new SynopsisModel();
            synopsisModel.UserId = Id;

            return View(synopsisModel);
        }

        // POST: Synopsis
        public ActionResult SaveSynopsisDetails(SynopsisModel objSynopsis)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            tbl_SynopsisDetails synopsisModel = new tbl_SynopsisDetails();
            synopsisModel.UserID = objSynopsis.UserId;
            synopsisModel.CategoryID = objSynopsis.CategoryId;
            synopsisModel.CollegeID = objSynopsis.CollegeId;
            synopsisModel.SynopsisHeader = objSynopsis.SynopsisHeader;
            synopsisModel.SynopsisDescription = objSynopsis.SynopsisDescription;
            std.tbl_SynopsisDetails.Add(synopsisModel);
            std.SaveChanges();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                //User Folder
                var UserFolder = "User_" + objSynopsis.UserId;
                string FilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"/Files/" + UserFolder);
                bool exists = Directory.Exists(Server.MapPath(FilePath));

                if (!exists)
                    Directory.CreateDirectory(Server.MapPath(FilePath));


                //Synopsis folder
                var SynopsisFolder = "Synopsis_" + synopsisModel.SynopsisID;
                string SynopsisFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"/Files/" + UserFolder + "/" + SynopsisFolder);
                bool SynopsisExists = Directory.Exists(Server.MapPath(SynopsisFilePath));

                if (!SynopsisExists)
                    Directory.CreateDirectory(Server.MapPath(SynopsisFilePath));


                var fileName = Request.Files[i].FileName;
                var filePath = SynopsisFilePath + "/" + fileName;
                var path = Path.Combine(Server.MapPath(filePath));
                Request.Files[i].SaveAs(path);


                tbl_FilesDetails fileDetails = new tbl_FilesDetails();
                fileDetails.SynopsisID = synopsisModel.SynopsisID;
                fileDetails.FileName = fileName;
                fileDetails.FilePath = filePath;

                std.tbl_FilesDetails.Add(fileDetails);
                std.SaveChanges();
            }

            return Json(new
            {
                IsValid = true,
                ResultMessage = "Project saved Successfully",
                Id = synopsisModel.SynopsisID,
                redirectToUrl = Url.Action("ViewFiles", "Synopsis", new { Id = objSynopsis.UserId })
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Synopsis
        public ActionResult ViewFiles(int Id, int? page)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }

            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            ProjectLibraryEntities std = new ProjectLibraryEntities();
            FilesListModel FileList = new FilesListModel();
            FileList.LoginUserId = Id;
            IPagedList<SynopsisModel> synopsysList = null;

            synopsysList = std.tbl_SynopsisDetails.AsEnumerable().Select((m, i) => new SynopsisModel()
            {
                Index = i + 1,
                SynopsisId = m.SynopsisID,
                UserId = m.UserID,
                CategoryId = m.CategoryID,
                CollegeId = m.CategoryID,

                UserName = m.tbl_Registration.UserName,
                CategoryName = m.tbl_CategoryMaster.CategoryName,
                CollegeName = m.tbl_CollegeMaster.CollegeName,

                SynopsisHeader = m.SynopsisHeader,
                SynopsisDescription = m.SynopsisDescription,
                Files = m.tbl_FilesDetails.AsEnumerable().Select(n => new FilesViewModel()
                {
                    FileID = n.FileID,
                    SynopsisID = n.SynopsisID,
                    FileName = n.FileName,
                    FilePath = n.FilePath
                }).ToList()
            }).ToPagedList(pageIndex, pageSize);
            FileList.SynopsysList = synopsysList;
            return View(FileList);
        }

        // POST: Synopsis
        public ActionResult EditSynopsisDetails(EditSynopsisModel objSynopsis)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            var SynopsysDetails = std.tbl_SynopsisDetails.Where(m => m.UserID == objSynopsis.UserId && m.SynopsisID == objSynopsis.SynopsisId).FirstOrDefault();
            if (SynopsysDetails != null)
            {

            }
            return Json(new
            {
                IsValid = true,
                ResultMessage = "Project saved Successfully",
                //Id = synopsisModel.SynopsisID,
                redirectToUrl = Url.Action("ViewFiles", "Synopsis", new { Id = objSynopsis.UserId })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download(int FileId)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            var FileDetails = std.tbl_FilesDetails.Where(m => m.FileID == FileId).FirstOrDefault();
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FileDetails.FilePath));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileDetails.FileName);

        }
        public ActionResult SearchProject(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();

            var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
            ViewData["Category"] = categoryMaster;

            var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
            ViewData["College"] = collegeMaster;
            SearchSynopsysModel FileList = new SearchSynopsysModel();
            FileList.LoginUserId = Id;
            return View(FileList);

        }

        [HttpPost]
        public ActionResult SearchFiles(int Id, SearchModel ObjSearch)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            ProjectLibraryEntities std = new ProjectLibraryEntities();

            var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
            ViewData["Category"] = categoryMaster;

            var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
            ViewData["College"] = collegeMaster;
            SearchSynopsysModel FileList = new SearchSynopsysModel();
            FileList.LoginUserId = Id;
            List<SynopsisModel> synopsysList = null;

            List<tbl_SynopsisDetails> filteredList = new List<tbl_SynopsisDetails>();
            if (ObjSearch.CategoryId > 0)
            {
                var searchedList = std.tbl_SynopsisDetails.Where(m => m.CategoryID == ObjSearch.CategoryId).ToList();
                filteredList.AddRange(searchedList);
            }
            if (ObjSearch.CollegeID > 0)
            {
                var searchedList = std.tbl_SynopsisDetails.Where(m => m.CollegeID == ObjSearch.CollegeID).ToList();
                filteredList.AddRange(searchedList);
            }
            if (!string.IsNullOrEmpty(ObjSearch.UserName))
            {
                var UserIds=std.tbl_Registration.Where(m => m.UserName.ToLower().Contains(ObjSearch.UserName.ToLower())).Select(m=>m.UserID).ToList();
                var searchedList = std.tbl_SynopsisDetails.Where(m => UserIds.Contains((int)m.UserID)).ToList();
                filteredList.AddRange(searchedList);
            }
            if (!string.IsNullOrEmpty(ObjSearch.SynopsisHeader))
            {
                var searchedList = std.tbl_SynopsisDetails.Where(m => m.SynopsisHeader.ToLower().Contains(ObjSearch.SynopsisHeader.ToLower())).ToList();
                filteredList.AddRange(searchedList);
            }
            if (!string.IsNullOrEmpty(ObjSearch.SynopsisDescription))
            {
                var searchedList = std.tbl_SynopsisDetails.Where(m => m.SynopsisDescription.ToLower().Contains(ObjSearch.SynopsisDescription.ToLower())).ToList();
                filteredList.AddRange(searchedList);
            }

            FileList.SynopsysList = filteredList.Distinct().AsEnumerable().Select((m, i) => new SynopsisModel()
            {
                Index = i + 1,
                SynopsisId = m.SynopsisID,
                UserId = m.UserID,
                CategoryId = m.CategoryID,
                CollegeId = m.CategoryID,

                UserName = m.tbl_Registration.UserName,
                CategoryName = m.tbl_CategoryMaster.CategoryName,
                CollegeName = m.tbl_CollegeMaster.CollegeName,

                SynopsisHeader = m.SynopsisHeader,
                SynopsisDescription = m.SynopsisDescription,
                Files = m.tbl_FilesDetails.AsEnumerable().Select(n => new FilesViewModel()
                {
                    FileID = n.FileID,
                    SynopsisID = n.SynopsisID,
                    FileName = n.FileName,
                    FilePath = n.FilePath
                }).ToList()
            }).ToList();
            return Json(new { Data = FileList }, JsonRequestBehavior.AllowGet);
        }
    }
}
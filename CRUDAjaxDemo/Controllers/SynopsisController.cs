﻿using CRUDAjaxDemo.Models;
using CRUDAjaxDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace CRUDAjaxDemo.Controllers
{
    public class SynopsisController : Controller
    {
        // GET: Synopsis
        public ActionResult Index(int Id)
        {
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
            }, JsonRequestBehavior.AllowGet);
        }


        // GET: Synopsis
        public ActionResult ViewFiles(int Id)
        {
            ProjectLibraryEntities std = new ProjectLibraryEntities();
            FilesListModel FileList = new FilesListModel();
            FileList.LoginUserId = Id;
            FileList.SynopsysList = std.tbl_SynopsisDetails.Where(m => m.UserID == Id).Select(m => new SynopsisModel()
            {
                SynopsisId = m.SynopsisID,
                UserId = m.UserID,
                CategoryId = m.CategoryID,
                CollegeId = m.CategoryID,

                UserName = m.tbl_Registration.UserName,
                CategoryName = m.tbl_CategoryMaster.CategoryName,
                CollegeName = m.tbl_CollegeMaster.CollegeName,

                SynopsisHeader = m.SynopsisHeader,
                SynopsisDescription = m.SynopsisDescription,
                Files = m.tbl_FilesDetails.Select(n => new FilesViewModel()
                {
                    FileID = n.FileID,
                    SynopsisID = n.SynopsisID,
                    FileName = n.FileName,
                    FilePath = n.FilePath
                }).ToList()
            }).ToList();

            return View(FileList);
        }
    }
}
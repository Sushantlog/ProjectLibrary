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
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
                ViewData["Category"] = categoryMaster;

                var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
                ViewData["College"] = collegeMaster;

                SynopsisModel synopsisModel = new SynopsisModel();
                synopsisModel.UserId = Id;
                synopsisModel.SynopsisId = 0;
                synopsisModel.CollegeId = 0;
                synopsisModel.CategoryId = 0;
                synopsisModel.Files = new List<FilesViewModel>();

                return View(synopsisModel);
            }
        }

        // POST: Synopsis
        public ActionResult SaveSynopsisDetails(SynopsisModel objSynopsis)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                if (!objSynopsis.IsEdit)
                {
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
                else
                {
                    tbl_SynopsisDetails synopsisModel = std.tbl_SynopsisDetails.Where(m => m.SynopsisID == objSynopsis.SynopsisId).FirstOrDefault();
                    synopsisModel.CategoryID = objSynopsis.CategoryId;
                    synopsisModel.CollegeID = objSynopsis.CollegeId;
                    synopsisModel.SynopsisHeader = objSynopsis.SynopsisHeader;
                    synopsisModel.SynopsisDescription = objSynopsis.SynopsisDescription;
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
                        var SynopsisFolder = "Synopsis_" + objSynopsis.SynopsisId;
                        string SynopsisFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"/Files/" + UserFolder + "/" + SynopsisFolder);
                        bool SynopsisExists = Directory.Exists(Server.MapPath(SynopsisFilePath));

                        if (!SynopsisExists)
                            Directory.CreateDirectory(Server.MapPath(SynopsisFilePath));


                        var fileName = Request.Files[i].FileName;
                        var filePath = SynopsisFilePath + "/" + fileName;
                        var path = Path.Combine(Server.MapPath(filePath));
                        Request.Files[i].SaveAs(path);


                        tbl_FilesDetails fileDetails = new tbl_FilesDetails();
                        fileDetails.SynopsisID = objSynopsis.SynopsisId;
                        fileDetails.FileName = fileName;
                        fileDetails.FilePath = filePath;

                        std.tbl_FilesDetails.Add(fileDetails);
                        std.SaveChanges();
                    }

                    return Json(new
                    {
                        IsValid = true,
                        ResultMessage = "Project edited Successfully",
                        Id = synopsisModel.SynopsisID,
                        redirectToUrl = Url.Action("ViewFiles", "Synopsis", new { Id = objSynopsis.UserId })
                    }, JsonRequestBehavior.AllowGet);

                }
            }
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

            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                FilesListModel FileList = new FilesListModel();
                FileList.LoginUserId = Id;
                var list = std.tbl_SynopsisDetails.Select(m => new SynopsisModel()
                {
                    SynopsisId = m.SynopsisID,
                    UserId = m.UserID,
                    CategoryId = m.CategoryID,
                    CollegeId = m.CollegeID,
                    SynopsisHeader = m.SynopsisHeader,
                    SynopsisDescription = m.SynopsisDescription
                }).ToList();

                int i = 1;

                foreach (var item in list)
                {
                    item.Index = i++;
                    item.UserName = std.tbl_Registration.Where(m => m.UserID == item.UserId).Select(m => m.UserName).FirstOrDefault();
                    item.CategoryName = std.tbl_CategoryMaster.Where(m => m.CategoryID == item.CategoryId).Select(m => m.CategoryName).FirstOrDefault();
                    item.CollegeName = std.tbl_CollegeMaster.Where(m => m.CollegeID == item.CollegeId).Select(m => m.CollegeName).FirstOrDefault();
                    item.Files = std.tbl_FilesDetails.Where(x => x.SynopsisID == item.SynopsisId).Select(n => new FilesViewModel()
                    {
                        FileID = n.FileID,
                        SynopsisID = n.SynopsisID,
                        FileName = n.FileName,
                        FilePath = n.FilePath
                    }).ToList();
                }

                FileList.SynopsysList = list.ToPagedList(pageIndex, pageSize); ;
                return View(FileList);
            }
        }

        // POST: Synopsis
        public ActionResult EditSynopsisDetails(EditSynopsisModel objSynopsis)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
                ViewData["Category"] = categoryMaster;

                var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
                ViewData["College"] = collegeMaster;

                var SynopsysDetails = std.tbl_SynopsisDetails.Where(m => m.UserID == objSynopsis.UserId && m.SynopsisID == objSynopsis.SynopsisId).FirstOrDefault();
                SynopsisModel ObjSynopsys = new SynopsisModel();
                if (SynopsysDetails != null)
                {
                    ObjSynopsys.UserId = SynopsysDetails.UserID;
                    ObjSynopsys.SynopsisId = SynopsysDetails.SynopsisID;
                    ObjSynopsys.CategoryId = SynopsysDetails.CategoryID;
                    ObjSynopsys.CollegeId = SynopsysDetails.CollegeID;
                    ObjSynopsys.SynopsisHeader = SynopsysDetails.SynopsisHeader;
                    ObjSynopsys.SynopsisDescription = SynopsysDetails.SynopsisDescription;
                    ObjSynopsys.Files = SynopsysDetails.tbl_FilesDetails.Select(m => new FilesViewModel
                    {
                        FileID = m.FileID,
                        SynopsisID = m.SynopsisID,
                        FileName = m.FileName,
                        FilePath = Server.MapPath(m.FilePath)
                    }).ToList();
                }
                return View("Index", ObjSynopsys);

            }
        }

        public ActionResult Download(int FileId)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var FileDetails = std.tbl_FilesDetails.Where(m => m.FileID == FileId).FirstOrDefault();
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FileDetails.FilePath));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileDetails.FileName);
            }
        }

        public ActionResult DeleteFile(int FileId)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var IsValid = false;
                var ResultMessage = "";
                var FileDetails = std.tbl_FilesDetails.Where(m => m.FileID == FileId).FirstOrDefault();
                if (FileDetails != null)
                {
                    std.tbl_FilesDetails.Remove(FileDetails);
                    std.SaveChanges();

                    IsValid = true;
                    ResultMessage = "File Deleted successfully!";
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

        public ActionResult DeleteSynopsys(int? SynopsysId)
        {
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var IsValid = false;
                var ResultMessage = "";
                var SynopsysDetails = std.tbl_SynopsisDetails.Where(m => m.SynopsisID == SynopsysId).FirstOrDefault();

                if (SynopsysDetails != null)
                {
                    var FileDetails = std.tbl_FilesDetails.Where(m => m.SynopsisID == SynopsysId).ToList();
                    std.tbl_FilesDetails.RemoveRange(FileDetails);
                    std.SaveChanges();

                    std.tbl_SynopsisDetails.Remove(SynopsysDetails);
                    std.SaveChanges();

                    IsValid = true;
                    ResultMessage = "Project Deleted successfully!";
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
        public ActionResult SearchProject(int Id)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var CategoryList = std.tbl_CategoryMaster.ToList();
                var categoryMaster = new SelectList(CategoryList, "CategoryID", "CategoryName");
                ViewData["Category"] = categoryMaster;

                var CollegeList = std.tbl_CollegeMaster.ToList();
                var collegeMaster = new SelectList(CollegeList, "CollegeID", "CollegeName");
                ViewData["College"] = collegeMaster;

                SearchSynopsysModel FileList = new SearchSynopsysModel();
                FileList.LoginUserId = Id;
                FileList.DefaultCategory = CategoryList.Select(m => m.CategoryID).FirstOrDefault();
                FileList.DefaultCollege = CollegeList.Select(m => m.CollegeID).FirstOrDefault();

                return View(FileList);
            }
        }

        [HttpPost]
        public ActionResult SearchFiles(int Id, SearchModel ObjSearch)
        {
            if (Session["UserID"] != null && Session["UserID"].ToString() != Id.ToString())
            {
                return RedirectToAction("Index", "login");
            }
            using (ProjectLibraryEntities std = new ProjectLibraryEntities())
            {
                var categoryMaster = new SelectList(std.tbl_CategoryMaster.ToList(), "CategoryID", "CategoryName");
                ViewData["Category"] = categoryMaster;

                var collegeMaster = new SelectList(std.tbl_CollegeMaster.ToList(), "CollegeID", "CollegeName");
                ViewData["College"] = collegeMaster;
                SearchSynopsysModel FileList = new SearchSynopsysModel();

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
                    var UserIds = std.tbl_Registration.Where(m => m.UserName.ToLower().Contains(ObjSearch.UserName.ToLower())).Select(m => m.UserID).ToList();
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

                FileList.LoginUserId = Id;
                FileList.PageIndex = ObjSearch.PageIndex;
                FileList.PageSize = 10;
                int startIndex = (ObjSearch.PageIndex - 1) * FileList.PageSize;
                FileList.RecordCount = filteredList.Distinct().ToList().Count();
                var list = filteredList.Distinct().Select((m, i) => new { Index = i, Model = m }).Skip(startIndex).Take(FileList.PageSize);


                FileList.SynopsysList = list.Select(synp => new SynopsisModel()
                {
                    Index = synp.Index + 1,
                    SynopsisId = synp.Model.SynopsisID,
                    UserId = synp.Model.UserID,
                    CategoryId = synp.Model.CategoryID,
                    CollegeId = synp.Model.CategoryID,

                    UserName = synp.Model.tbl_Registration.UserName,
                    CategoryName = synp.Model.tbl_CategoryMaster.CategoryName,
                    CollegeName = synp.Model.tbl_CollegeMaster.CollegeName,

                    SynopsisHeader = synp.Model.SynopsisHeader,
                    SynopsisDescription = synp.Model.SynopsisDescription,
                    Files = synp.Model.tbl_FilesDetails.Select(n => new FilesViewModel()
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
}
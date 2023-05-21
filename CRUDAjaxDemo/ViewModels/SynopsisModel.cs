using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CRUDAjaxDemo.ViewModels
{
    public class SynopsisModel
    {
        public int Index { get; set; }
        public int? SynopsisId { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? CollegeId { get; set; }

        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public string CollegeName { get; set; }

        public string SynopsisHeader { get; set; }
        public string SynopsisDescription { get; set; }

        public List<FilesViewModel> Files { get; set; }
    }

    public class FilesListModel
    {
        public int LoginUserId { get; set; }

        public IPagedList<SynopsisModel> SynopsysList { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

    }

    public class EditSynopsisModel
    {
        public int UserId { get; set; }

        public int SynopsisId { get; set; }
    }
    public class SearchModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? CategoryId { get; set; }
        public int? CollegeID { get; set; }
        public string SynopsisHeader { get; set; }
        public string SynopsisDescription { get; set; }
    }
}

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

        public List<SynopsisModel> SynopsysList { get; set; }
    }
}

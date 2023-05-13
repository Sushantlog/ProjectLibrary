using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class FilesViewModel
    {
        public int? FileID { get; set; }
        public int? SynopsisID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
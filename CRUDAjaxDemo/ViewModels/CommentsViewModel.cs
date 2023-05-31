using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class CommentsViewModel
    {
        public int Index { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string CommentHeader { get; set; }
        public string CommentDescription { get; set; }
        public bool IsEdit { get; set; }
    }

    public class ShowCommentsViewModel
    {
        public int Index { get; set; }
        public List<CommentsViewModel> CommentList { get; set; }
    }
}
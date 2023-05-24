using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class CommentsViewModel
    {
        
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string CommentHeader { get; set; }
        public string CommentDescription { get; set; }
    }
}
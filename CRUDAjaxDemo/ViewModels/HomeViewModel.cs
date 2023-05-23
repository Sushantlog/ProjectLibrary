using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class HomeViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public CounterModel counter { get; set; }
    }

    public class CounterModel
    {
        public int TotalUsers { get; set; }
        public int TotalProjects { get; set; }
        public int TotalFiles { get; set; }
        public int TotalActiveUsers { get; set; }

    }
}
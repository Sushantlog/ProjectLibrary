using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
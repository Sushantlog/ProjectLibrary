using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class LoginViewModel
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
    }
}
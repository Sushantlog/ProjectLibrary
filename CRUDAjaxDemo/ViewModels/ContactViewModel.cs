using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDAjaxDemo.ViewModels
{
    public class ContactViewModel
    {
        public int LoginUserId { get; set; }
        public string SupportEmail { get; set; }
        public List<ContactDetailsViewModel> ContactList { get; set; }
    }

    public class ContactDetailsViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }

    public class MailModel
    {
        public int UserId { get; set; }
        public string Message { get; set; }
    }
}
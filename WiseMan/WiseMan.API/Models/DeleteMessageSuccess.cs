using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class DeleteMessageSuccess
    {
        public string Message { get; set; }

        public System.Net.HttpStatusCode HttpStatusCode { get; set; }
    }
}
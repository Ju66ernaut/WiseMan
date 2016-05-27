using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class ErrorResult
    {
        public string ErrorMessage { get; set; }

        public System.Net.HttpStatusCode HttpStatusCode { get; set; }

        //
    }
}
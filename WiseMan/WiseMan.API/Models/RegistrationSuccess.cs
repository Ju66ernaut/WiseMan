using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class RegistrationSuccess
    {
        public string SuccessMessage { get; set; }

        public Token Token { get; set; }
    }
}
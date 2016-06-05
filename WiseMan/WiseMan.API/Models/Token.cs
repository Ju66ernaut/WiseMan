using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class Token
    {
        private string v;

        public Token(string encodedToken)
        {
            this.JWT = encodedToken;
        }

        public string JWT { get; set; }
    }
}
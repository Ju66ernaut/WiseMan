using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class NewMessage
    {
        //public Guid Id { get; set; }

        public string Body { get; set; }

        public Guid AuthorId { get; set; }
        
        public List<string> Tags { get; set; }
    }
}
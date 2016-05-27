using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public User Author { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
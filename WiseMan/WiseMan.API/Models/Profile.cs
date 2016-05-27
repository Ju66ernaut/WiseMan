using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class Profile
    {
        public Guid UserId { get; set; }

        public string Header { get; set; }

        public Message HighestRated { get; set; }

        public string Username { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; } //??

        //what else is on a profile???
    }
}
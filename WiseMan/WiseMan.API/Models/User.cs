using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiseMan.API.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public List<Message> Favorites { get; set; }
    }
}
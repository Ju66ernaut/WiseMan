using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Data;

namespace WiseMan.API.Models
{
    public class User
    {
        private GetUserByUsername_Result resultSet;
        private GetUserSaltHash_Result result;

        public User()
        {

        }
        public User(GetUserByUsername_Result resultSet)
        {
            this.Id = resultSet.UserId;
            this.FirstName = resultSet.FirstName;
            this.LastName = resultSet.LastName;
            this.Username = resultSet.Username;
            this.Email = resultSet.Email;
        }

        public User(GetUserSaltHash_Result resultSet)
        {
            //this will need to be more robust and descriptive in the future like, isLockedOut, isActive, etc
            this.Id = resultSet.UserId;
            this.FirstName = resultSet.FirstName;
            this.LastName = resultSet.LastName;
            this.Username = resultSet.Username;
            this.Email = resultSet.Email;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Username { get; set; }

        public List<Message> Favorites { get; set; }

        public Guid ProfileId { get; set; }
    }
}
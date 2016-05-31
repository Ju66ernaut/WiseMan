using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Data;

namespace WiseMan.API.Models
{
    public class Message
    {
        private GetMessageById_Result getMessageById_Result;

        public Message()
        {

        }
        public Message(GetMessageById_Result result)
        {
            this.Id = result.MessageId;
            this.Body = result.Body;
            this.Upvotes = result.Upvotes;
            this.Downvotes = result.Downvotes;
            this.Tags = new List<Tag>();
            foreach (var item in result.Tags.Split(','))
            {
                this.Tags.Add(new Tag() { Name = item });
            }    
            this.Author = new User()
            {
                Id = result.AuthorId,
                Username = result.Username
            };

        }

        public Guid Id { get; set; }

        public string Body { get; set; }

        public User Author { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
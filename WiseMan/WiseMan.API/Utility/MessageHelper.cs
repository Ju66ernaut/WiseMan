using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Models;

namespace WiseMan.API.Utility
{
    /// <summary>
    /// To serve as the DAL for message related actions
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="body"></param>
        /// <param name="authorId"></param>
        /// <param name="tags"></param>
        public static void CreateMessage(string messageBody, Guid authorId, List<string> tags)
        {
            string tagString = "";
            foreach (var item in tags)
            {
                tagString += item + ",";
            }
            using(Data.WiseManEntities db = new Data.WiseManEntities())
            {
                db.CreateMessage(messageBody, tagString, authorId);
            }
        }

        /// <summary>
        /// Gets a message by id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        internal static Message GetMessageById(Guid messageId)
        {
            Message message;
            using(Data.WiseManEntities db = new Data.WiseManEntities())
            {
                message = new Message(db.GetMessageById(messageId).FirstOrDefault());
            }
            return message;
        }

        internal static List<Message> GetMessagesByAuthorId(Guid authorId)
        {
            List<Message> messages = new List<Message>();
            using(Data.WiseManEntities db = new Data.WiseManEntities())
            {
                var results = db.GetMessagesByAuthor(authorId).ToList();
                foreach (var item in results)
                {
                    List<Tag> tags = new List<Tag>();
                    List<string> tagNames = item.TagArray.Split(',').ToList();
                    foreach (var tag in tagNames)
                    {
                        tags.Add(new Tag()
                        {
                            Name = tag
                        });
                    }
                    messages.Add(new Message()
                    {
                        Body = item.Body,
                        Downvotes = item.Downvotes,
                        Id = item.MessageId,
                        Upvotes = item.Upvotes,
                        Tags = tags
                    });
                }
            }
            return messages;
        }

        internal static void DeleteMessage(Guid authorId, Guid messageId)
        {
            using(Data.WiseManEntities db = new Data.WiseManEntities())
            {
                db.DeleteMessage(messageId, authorId);
            }
        }
    }
}
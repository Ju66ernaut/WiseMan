using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WiseMan.API.Models;

namespace WiseMan.API.Controllers
{
    [RoutePrefix("api/v1/messages")]
    public class MessageController : BaseController
    {
        /// <summary>
        /// Gets a message by Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [Route("id/{messageId}"), HttpGet, ResponseType(typeof(Message))]
        public IHttpActionResult GetMessage(Guid messageId)
        {
            if (messageId == null)
            {
                //return error message
                return BadRequest("messageId is null");
            }
            try
            {
                return new ApiQueryResult<Message>(this.Request)
                {
                    //Content = message
                };
            }
            catch (Exception)
            {
                //need error handling
                //http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling 
                throw;
            }
        }

        /// <summary>
        /// Gets messages created by the user with the given authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [Route("authorId/{authorId}"), HttpGet, ResponseType(typeof(List<Message>))]
        public IHttpActionResult GetMessagesByAuthor(Guid authorId)
        {
            if (authorId == null)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="authorId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("new"), HttpPost]
        public IHttpActionResult CreateMessage(string message, Guid authorId, List<Tag> tags)
        {
            if (string.IsNullOrEmpty(message) || authorId == null || tags.Count == 0)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Updates the tags of the given message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="tags"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [Route("modifytags/{messageId}"),HttpPost]
        public IHttpActionResult UpdateMessageTags(Guid messageId, List<Tag> tags, Guid authorId)
        {
            if(messageId == null || tags.Count == 0 || authorId == null)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Deletes the message with the given Id
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [Route("delete/{messageId}"), HttpDelete]
        public IHttpActionResult DeleteMessage(Guid authorId, Guid messageId)
        {
            if (authorId == null || messageId == null)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Adds 1 to the upvote counter for the given message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("upvote/{messageId}"), HttpPost]
        public IHttpActionResult UpvoteMessage(Guid messageId, Guid userId)
        {
            //may not need userId if i decide to use JWT
            if (messageId == null || userId == null)
            {
                //
            }
            return null;
        }
        /// <summary>
        /// Adds 1 to the downvote counter for the given message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("upvote/{messageId}"), HttpPost]
        public IHttpActionResult DownvoteMessage(Guid messageId, Guid userId)
        {
            //may not need userId if i decide to use JWT
            if (messageId == null || userId == null)
            {

            }
            return null;
        }

        /// <summary>
        /// Gets the favorites for the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("favorites/{userId}"), HttpGet, ResponseType(typeof(List<Message>))]
        public IHttpActionResult GetFavorites(Guid userId)
        {
            //may not need userId if i decide to use JWT
            if(userId == null)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Gets all messages with the given tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route("tag/{tagId}"), HttpGet, ResponseType(typeof(List<Message>))]
        public IHttpActionResult GetMessagesByTag(Guid tagId)
        {
            if(tagId == null)
            {

            }
            return null;

            //return sorted list of messages
        }

        /// <summary>
        /// Gets messages based on the given method (top, new)
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("method/{methodName}"), HttpGet, ResponseType(typeof(List<Message>))]
        public IHttpActionResult GetMessagesByMethod(string methodName, Guid userId)
        {
            //not sure about this implementation
            //top, new
            if(string.IsNullOrEmpty(methodName) || userId == null)
            {
                //
            }
            return null;
        }
    }
}

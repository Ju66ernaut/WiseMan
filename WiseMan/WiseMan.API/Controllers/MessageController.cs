using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WiseMan.API.Filters;
using WiseMan.API.Models;
using WiseMan.API.Utility;

namespace WiseMan.API.Controllers
{
    [CustomAuthorization]
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
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);
            if (messageId == null)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = "messageId is null",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
                return exResult;
            }
            try
            {
                Message message = MessageHelper.GetMessageById(messageId);

                return new ApiResult<Message>(this.Request)
                {
                    Content = message,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                //maybe better error handling?
                //http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling 
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
                return exResult;
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
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);
            if (authorId == null)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "authorId is null" };
                return exResult;
            }

            try
            {
                List<Message> messagesByAuthor = MessageHelper.GetMessagesByAuthorId(authorId);
                ApiResult<List<Message>> messages = new ApiResult<List<Message>>(this.Request)
                {
                    Content = messagesByAuthor,
                    StatusCode = HttpStatusCode.OK
                };
                return messages;
            }
            catch (Exception ex)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
                return exResult;
            }
        }

        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        [Route("new"), HttpPost]
        public IHttpActionResult CreateMessage(NewMessage newMessage)
        {
            //TODO
            //restrict tag length
            //no spaces in tags (tell users to use '-')
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);
            if (string.IsNullOrEmpty(newMessage.Body))
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "message body can not be empty" };
                return exResult;
            }
            if (newMessage.AuthorId == null)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "authorId is null" };
                return exResult;
            }
            if (newMessage.Tags.Count == 0 || newMessage.Tags.Count > 3)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "messages can only have up to 3 tags" };
                return exResult;
            }
            if (newMessage.Tags.Any(x => string.IsNullOrEmpty(x)))
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "tags cannot be empty" };
                return exResult;
            }
            if (newMessage.Tags.Any(x => x.Any(chr => char.IsDigit(chr))))
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "tags cannot contain numbers" };
                return exResult;
            }

            try
            {
                //TODO:
                //check DB for last post
                //if user points amount is under X amount, limit the post intervals
                ////if last post was within 8 min, reject the current post
                List<string> tagsLower = newMessage.Tags.Select(t => t.ToLowerInvariant()).ToList();

                MessageHelper.CreateMessage(newMessage.Body, newMessage.AuthorId, tagsLower); //get tagIds out?

                //return new message data back?

                //what data needs to be returned here? return Message or NewMessage data
                ApiResult<Message> message = new ApiResult<Message>(this.Request)
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new Message()
                    {
                        Body = newMessage.Body,
                        Downvotes = 0,
                        Upvotes = 1,
                        //Tags = tags
                    }
                };
                return message;
            }
            catch (Exception ex)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
                return exResult;
            }
        }

        /// <summary>
        /// Deletes the message with the given Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [Route("delete/{messageId}"), HttpDelete]
        public IHttpActionResult DeleteMessage(Guid messageId)
        {
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);
     
            if (messageId == null)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "messageId cannot be null" };
                return exResult;
            }

            try
            {
                User user = Request.Properties["user"] as User;
  
                MessageHelper.DeleteMessage(user.Id, messageId);

                return new ApiResult<DeleteMessageSuccess>(this.Request)
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new DeleteMessageSuccess()
                    {
                        HttpStatusCode = HttpStatusCode.Accepted,
                        Message = "Message deleted"
                    }
                };
            }
            catch (Exception ex)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
                return exResult;
            }
        }

        /// <summary>
        /// Adds 1 to the upvote counter for the given message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("upvote/{messageId}"), HttpPost]
        public IHttpActionResult UpvoteMessage(Guid messageId)
        {
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);
     
            if (messageId == null)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "messageId cannot be null" };
                return exResult;
            }
            User user = Request.Properties["user"] as User;
            try
            {
                MessageHelper.UpvoteMessage(messageId, user.Id);
                return new ApiResult<string>(this.Request) { StatusCode = HttpStatusCode.OK, Content = "Message upovted" };
            }
            catch (Exception ex)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
                return exResult;
            }     
     
        }
        /// <summary>
        /// Adds 1 to the downvote counter for the given message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("downvote/{messageId}"), HttpPost]
        public IHttpActionResult DownvoteMessage(Guid messageId, Guid userId)
        {
            ApiResult<ErrorResult> exResult = new ApiResult<ErrorResult>(this.Request);

            if (messageId == null)
            {
                exResult.Content = new ErrorResult() { HttpStatusCode = HttpStatusCode.BadRequest, ErrorMessage = "messageId cannot be null" };
                return exResult;
            }
            User user = Request.Properties["user"] as User;
            try
            {
                MessageHelper.DownvoteMessage(messageId, user.Id);
                return new ApiResult<string>(this.Request) { StatusCode = HttpStatusCode.OK, Content = "Message downvoted" };
            }
            catch (Exception ex)
            {
                exResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
                return exResult;
            }
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
            if (userId == null)
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
        public IHttpActionResult GetMessagesByTags(Guid tagId)
        {
            //take in array of tags?
            if (tagId == null)
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
            if (string.IsNullOrEmpty(methodName) || userId == null)
            {
                //
            }
            return null;
        }
    }
}

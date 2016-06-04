using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WiseMan.API.Filters;
using WiseMan.API.Models;

namespace WiseMan.API.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        /// <summary>
        /// Gets the profile for the selected author
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("profile/{authorId}"), HttpGet, ResponseType(typeof(Profile))]
        public IHttpActionResult GetProfile(Guid authorId)
        {
            if(authorId == null)
            {
                //
            }
            return null;
        }

        //TODO

        //[CustomAuthorization]
        //[Route("user"), HttpGet, ResponseType(typeof(UserInfo))]
        //public IHttpActionResult GetUserInfo()
        //{
        //    try
        //    {
        //        User user = Request.Properties["user"] as User;

        //        UserInfo content = new UserInfo(user);

        //        return new ApiResult<UserInfo>(this.Request)
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = content
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}

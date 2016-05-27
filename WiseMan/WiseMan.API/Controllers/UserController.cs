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
        public IHttpActionResult GetProfile(Guid authorId, Guid userId)
        {
            if(authorId == null || userId == null)
            {
                //
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WiseMan.API.Models;
using WiseMan.API.Providers;
using WiseMan.API.Results;
using WiseMan.API.Utility;
using System.Web.Http.Description;
using System.Web.Helpers;

namespace WiseMan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        //private const string LocalLoginProvider = "Local";
        //private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

  
        [Route("userinfo")]
        public UserInfoViewModel GetUserInfo()
        {
            return null;
        }

        // POST api/Account/Logout
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            return null;
        }
             

        // POST api/Account/ChangePassword
        [Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        

        // POST api/Account/Register
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register"), HttpPost, ResponseType(typeof(ApiQueryResult<string>))]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApiQueryResult<RegistrationSuccess> result = new ApiQueryResult<RegistrationSuccess>(this.Request);
            try
            {
                AccountHelper.RegisterNewAccount(model.Username, model.Password, model.Email);
                result.Content = new RegistrationSuccess()
                {
                    SuccessMessage = "New account successfully registered",
                    Token = "hfihsfihfinsdfjiidf"
                };
                result.StatusCode = System.Net.HttpStatusCode.OK;
                return result;
                //this should also authenticate the user and pass back a JWT
                //make json object with message and JWT?
            }
            catch (Exception ex)
            {
                ApiQueryResult<ErrorResult> exceptionResult = new ApiQueryResult<ErrorResult>(this.Request);
                exceptionResult.Content = new ErrorResult()
                {
                    ErrorMessage = ex.InnerException.Message,
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
                exceptionResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return exceptionResult;
            }

        }
           
    
        #region Helpers
        
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WiseMan.API.Models;
using WiseMan.API.Utility;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Net;

namespace WiseMan.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {

        public AccountController()
        {
        }

        [AllowAnonymous]
        [Route("login"), HttpPost, ResponseType(typeof(JWT.JsonWebToken))]
        public IHttpActionResult Login(Login model)
        {
      
            if (string.IsNullOrEmpty(model.Username))
            {
                return BadRequest("Username is required");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Password is required");
            }
            try
            {
                ////testing usertoken
                //UserToken testToken = new UserToken(model.Username, model.Password, DateTime.Now.AddHours(1));
                //var testTokenStr = testToken.ToHexString();

                User user = null;
                var returnContent = new ApiResult<object>(this.Request);

                if (AccountHelper.ValidateUser(model.Username, model.Password, out user))
                {
                    var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var weekFromNow = Math.Round((DateTime.UtcNow.AddDays(7) - unixEpoch).TotalSeconds);

                    var payload = new Dictionary<string, object>()
                        {
                            { "iss", "wiseman" },
                            { "exp", weekFromNow },
                            { "sub", model.Username }
                            //"scopes", roles ?                                                                     
                        };

                    var secretKey = System.Configuration.ConfigurationManager.AppSettings["appKey"];

                    //TODO
                    //var token = new Token(JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256));

                    //TODO
                    //returnContent.Content = token;

                    return null;
                }
                else
                {
                    if (user != null)
                    {
                        //TODO
                        //if (user.IsLockedOut)
                        //{
                        //    returnContent.StatusCode = HttpStatusCode.Forbidden;
                        //    returnContent.Content = "Account locked out. Please contact your administrator";
                        //}
                        //if (!user.IsApproved)
                        //{
                        //    returnContent.StatusCode = HttpStatusCode.Forbidden;
                        //    returnContent.Content = "Account access has been denied. Please contact your administrator";
                        //}
                    }
                    returnContent.StatusCode = HttpStatusCode.NotFound;
                    returnContent.Content = "No user found with the provided credentials";
                }
                return returnContent;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
        [Route("register"), HttpPost, ResponseType(typeof(ApiResult<string>))]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApiResult<RegistrationSuccess> result = new ApiResult<RegistrationSuccess>(this.Request);
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
                ApiResult<ErrorResult> exceptionResult = new ApiResult<ErrorResult>(this.Request);
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

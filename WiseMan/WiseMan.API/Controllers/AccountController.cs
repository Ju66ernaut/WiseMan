using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WiseMan.API.Models;
using WiseMan.API.Utility;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Net;
using WiseMan.API.Filters;

namespace WiseMan.API.Controllers
{
    [CustomAuthorization]
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
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
               
                User user = null;
                var returnContent = new ApiResult<Object>(this.Request);

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
                    
                    returnContent.Content = new Token(JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256));

                    return returnContent;
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
        
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            return null;
        }
        
        [Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        
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
                result.StatusCode = HttpStatusCode.OK;
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
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
                exceptionResult.StatusCode = HttpStatusCode.BadRequest;
                return exceptionResult;
            }

        }

        
    }
}

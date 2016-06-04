using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WiseMan.API.Utility;

namespace WiseMan.API.Filters
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }

            HandleUnauthorizedRequest(actionContext);
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            if (!headers.Contains("Authorization"))
                return false;

            var tokenStr = headers.GetValues("Authorization").First();

            var secretKey = System.Configuration.ConfigurationManager.AppSettings["appKey"];

            string username = string.Empty;
            try
            {
                var payload = JWT.JsonWebToken.DecodeToObject(tokenStr, secretKey) as IDictionary<string, object>;

                username = payload["sub"].ToString();

            }
            catch (JWT.SignatureVerificationException ex)
            {
                return false;
            }
            if (string.IsNullOrEmpty(username))
                return false;


            //will having the user object in the context make the authorId parameter in endpoints obsolete?
            var user = AccountHelper.GetUserByUsername(username);


            //add user object to request -- this may be reduced to just unitId or something
            //TODO:
            //THIS MAY CHANGE
            actionContext.Request.Properties.Add("user", user);

            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            //invalid token message or something

            UnauthorizedResponse response = new UnauthorizedResponse();

            actionContext.Response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new ObjectContent<UnauthorizedResponse>(response, new JsonMediaTypeFormatter(), "application/json")
            };

        }
    }
}
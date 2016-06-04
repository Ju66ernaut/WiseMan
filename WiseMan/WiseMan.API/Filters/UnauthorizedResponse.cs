using System.Net;

namespace WiseMan.API.Filters
{    

    internal class UnauthorizedResponse
    {
        public UnauthorizedResponse()
        {
            this.Message = "Access denied";
            this.HttpStatusCode = HttpStatusCode.Unauthorized;
        }

        public string Message { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WiseMan.API.Models
{
    public class ApiResult<T>: IHttpActionResult where T : class
    {
        public ApiResult(HttpRequestMessage request)
        {
            this.StatusCode = HttpStatusCode.OK; ;
            this.HeadersToAdd = new List<MyStringPair>();
            this.Request = request;
        }

        public HttpStatusCode StatusCode { get; set; }
        private List<MyStringPair> HeadersToAdd { get; set; }
        public T Content { get; set; }
        private HttpRequestMessage Request { get; set; }

        public void AddHeaders(string headerKey, string headerValue)
        {
            this.HeadersToAdd.Add(new MyStringPair(headerKey, headerValue));
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.Request.CreateResponse<T>(this.StatusCode, this.Content);
            foreach (var hdr in this.HeadersToAdd)
            {
                response.Content.Headers.Add(hdr.key, hdr.value);
            }
            return Task.FromResult(response);
        }

        private class MyStringPair
        {
            public MyStringPair(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
            public string key;
            public string value;
        }
    }
}
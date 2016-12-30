using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WiseMan.API.Controllers
{
    [EnableCors(origins: "http://localhost:63247", headers: "*", methods: "*")]
    public class BaseController : ApiController
    {
    }
}

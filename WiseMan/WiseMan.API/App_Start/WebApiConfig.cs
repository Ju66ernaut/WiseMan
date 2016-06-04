using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WiseMan.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
    
            // Web API routes
            config.MapHttpAttributeRoutes();

            //removing support for XML because reasons
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //json stuff
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            //TODO
            //check if we need an all props resolver for nested json objects
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new AllPropertiesResolver();

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;


        }
    }
}

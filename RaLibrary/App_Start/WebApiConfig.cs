using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RaLibrary
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Only responses JSON format.
            // <https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/json-and-xml-serialization#removing_the_json_or_xml_formatter>
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes:
            // - api/user : UserController
            // - api/books : BooksController
            // <https://github.com/CVBDL/RALibraryDocs/blob/master/rest-api.md>
            config.MapHttpAttributeRoutes();
            
        }
    }
}

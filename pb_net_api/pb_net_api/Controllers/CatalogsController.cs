using Newtonsoft.Json.Linq;
using pb_net_api.EFModels;
using pb_net_api.PayBookCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace pb_net_api.Controllers
{
    public class CatalogsController : ApiController
    {
        // GET: api/Catalogs/Get
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string token)
        {
            string catalogs = "";
            Paybook paybook = new Paybook();

            catalogs = paybook.catalogs(token);

            JToken json = JObject.Parse("{ 'catalogs' : '" + catalogs + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
using Newtonsoft.Json.Linq;
using pb_net_api.EFModels;
using PaybookSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace pb_net_api.Controllers
{
    public class AccountsController : ApiController
    {
        // GET: api/Accounts/Get
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string token, string id_site)
        {
            string accounts = "";
            try
            {
                Paybook paybook = new Paybook();
                accounts = paybook.accounts(token, id_site);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            JToken json = JObject.Parse("{ 'accounts' : '" + accounts + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
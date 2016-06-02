using Newtonsoft.Json.Linq;
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
    public class TransactionsController : ApiController
    {
        // GET: api/Transactions/Get
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string token, string id_account)
        {
            string transactions = "";
            try
            {
                Paybook paybook = new Paybook();

                transactions = paybook.transactions(token, id_account);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            JToken json = JObject.Parse("{ 'transactions' : '" + transactions + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
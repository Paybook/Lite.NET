using Newtonsoft.Json.Linq;
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
    public class TransactionsController : ApiController
    {
        // GET: api/Transactions/transactions
        [System.Web.Http.HttpGet]
        public HttpResponseMessage transactions(string token, string id_account)
        {
            string transactions = "";

            Paybook paybook = new Paybook();
            transactions = paybook.transactions(token, id_account);

            JToken json = JObject.Parse("{ 'transactions' : '" + transactions + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
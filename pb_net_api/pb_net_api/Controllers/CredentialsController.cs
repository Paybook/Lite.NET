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
    public class CredentialsController : ApiController
    {
        // POST: api/Credentials/credentials
        [System.Web.Http.HttpPost]
        //public HttpResponseMessage credentials(string token, string id_site, string credentials_user, string credentials_password)
        public HttpResponseMessage credentials(string token, string name, string id_site, string [] credentials_user)
        {
            string credentials = "false";
            PayBookEntities entities = new PayBookEntities();

            var user = entities.users.FirstOrDefault(u => u.token == token);

            if (user != null)
            {
                Paybook paybook = new Paybook();
                JObject new_credentials = paybook.credentials(token, name, id_site, credentials_user);

                if (new_credentials != null)
                {
                    entities.credentials.Add(new EFModels.credentials { ws = new_credentials["ws"].ToString(), status = new_credentials["status"].ToString(), twofa = new_credentials["twofa"].ToString(), id_credential = new_credentials["id_credential"].ToString() });
                    entities.SaveChanges();
                    credentials = new_credentials.ToString();
                }
            }

            JToken json = JObject.Parse("{ 'credentials' : '" + credentials + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }

        // GET: api/Credentials/Status
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Status(string token, string id_site)
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
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
    public class CredentialsController : ApiController
    {
        // POST: api/Credentials/Register
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Register(JObject credentials)
        {
            string credentialsResponse = "false";
            try
            {
                PayBookEntities entities = new PayBookEntities();

                string token = credentials["token"].ToObject<string>();
                string id_site = credentials["id_site"].ToObject<string>();

                var user = entities.users.FirstOrDefault(u => u.token == token);

                if (user != null)
                {
                    Paybook paybook = new Paybook();
                    JObject new_credentials = paybook.credentials(credentials);

                    if (new_credentials != null)
                    {
                        entities.credentials.Add(new EFModels.credentials { id_user = user.id_user, id_site = id_site, ws = new_credentials["ws"].ToString(), status = new_credentials["status"].ToString(), twofa = new_credentials["twofa"].ToString(), id_credential = new_credentials["id_credential"].ToString() });
                        entities.SaveChanges();
                        credentialsResponse = new_credentials.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            JToken json = JObject.Parse("{ 'credentials' : '" + credentialsResponse + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }

        // GET: api/Credentials/Status
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Status(string token, string id_site)
        {
            string status = "false";
            try
            {
                PayBookEntities entities = new PayBookEntities();

                var user = entities.users.FirstOrDefault(u => u.token == token);

                if (user != null)
                {
                    var credentials = entities.credentials.FirstOrDefault(c => c.id_user == user.id_user && c.id_site == id_site);
                    string url_status = credentials.status;

                    Paybook paybook = new Paybook();
                    status = paybook.status(token, id_site, url_status);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            JToken json = JObject.Parse("{ 'status' : '" + status + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
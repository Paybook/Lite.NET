using Newtonsoft.Json.Linq;
using PaybookSDK;
using pb_net_api.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace pb_net_api.Controllers
{
    public class UserController : ApiController
    {
        // POST: api/User/signup
        [System.Web.Http.HttpPost]
        public HttpResponseMessage signup(string username, string password)
        {
            string signed_up = "Error";
            try
            {
                PayBookEntities entities = new PayBookEntities();

                var user = entities.users.FirstOrDefault(u => u.username == username && u.password == password);

                if (user == null)
                {
                    //callling paybook sdk
                    Paybook paybook = new Paybook();
                    string id_user = paybook.signup(username);

                    //saving to database
                    entities.users.Add(new users { username = username, password = password, id_user = id_user, date = DateTime.Now.ToShortDateString() });
                    entities.SaveChanges();
                    signed_up = "true";
                }
                else signed_up = "true";

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            JToken json = JObject.Parse("{ 'signed_up' : '" + signed_up + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }

        // POST: api/User/login
        [System.Web.Http.HttpPost]
        public HttpResponseMessage login(string username, string password)
        {
            string token = "Error";
            try
            {
                PayBookEntities entities = new PayBookEntities();

                var user = entities.users.FirstOrDefault(u => u.username == username && u.password == password);

                if (user != null)
                {
                    Paybook paybook = new Paybook();
                    token = paybook.login(user.id_user);

                    user.token = token;
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            JToken json = JObject.Parse("{ 'token' : '" + token + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }
}
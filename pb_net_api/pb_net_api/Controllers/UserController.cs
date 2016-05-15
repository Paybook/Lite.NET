using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using pb_net_api.Models;
using pb_net_api.Providers;
using pb_net_api.Results;
using pb_net_api.EFModels;
using System.Linq;
using pb_net_api.PayBookCalls;
//using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using System.Net;

namespace pb_net_api.Controllers
{
    public class UserController : ApiController
    {
        // POST: api/User/signup
        [System.Web.Http.HttpPost]
        public HttpResponseMessage signup(string username, string password)
        {
            string signed_up = "Error";
            PayBookEntities entities = new PayBookEntities();

            var user = entities.users.FirstOrDefault(u => u.username == username && u.password == password);

            if (user == null)
            {
                Paybook paybook = new Paybook();
                string id_user = paybook.signup(username);

                entities.users.Add(new users { username = username, password = password, id_user = id_user, date = DateTime.Now.ToShortDateString() });
                entities.SaveChanges();
                signed_up = "true";
            }
            else signed_up = "true";

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
            PayBookEntities entities = new PayBookEntities();

            var user = entities.users.FirstOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                Paybook paybook = new Paybook();
                token = paybook.login(user.id_user);

                user.token = token;
                entities.SaveChanges();
            }
            JToken json = JObject.Parse("{ 'token' : '" + token + "' }");
            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }
    }

    public class JsonContent : HttpContent
    {
        private readonly JToken _value;

        public JsonContent(JToken value)
        {
            _value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream,
            TransportContext context)
        {
            var jw = new JsonTextWriter(new StreamWriter(stream))
            {
                Formatting = Formatting.Indented
            };
            _value.WriteTo(jw);
            jw.Flush();
            return Task.FromResult<object>(null);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
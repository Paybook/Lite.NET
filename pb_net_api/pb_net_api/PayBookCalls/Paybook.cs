using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Helpers;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace pb_net_api.PayBookCalls
{
    public class Paybook
    {
        string paybook_link;
        string api_key;
        public Paybook()
        {
            paybook_link = ConfigurationManager.AppSettings["PAYBOOK_LINK"];
            api_key = ConfigurationManager.AppSettings["API_KEY"];
        }
        public string signup(string username)
        {
            //parameters = "users?_method=post&api_key=" + api_key + "&name="+ username;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                data data = new data() { api_key = api_key, name = username };

                HttpResponseMessage response = client.PostAsJsonAsync("users", data).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject user = JObject.Parse(respon);
                    return user["response"]["id_user"].ToString();
                }
                else
                    return string.Empty;
            }
        }

        public string login(string id_user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                data data = new data() { api_key = api_key, id_user = id_user };

                HttpResponseMessage response = client.PostAsJsonAsync("sessions", data).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject user = JObject.Parse(respon);
                    return user["response"]["token"].ToString();
                }
                else
                    return string.Empty;
            }
        }

        public string catalogs(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET Using QueryStrings
                string parameters = "catalogues/sites?token=" + token;

                HttpResponseMessage response = client.GetAsync(parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject catalogs = JObject.Parse(respon);
                    return catalogs["response"].ToString();
                }
                else
                    return string.Empty;
            }
        }

        public JObject credentials(JObject newcredentials)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.PostAsync("credentials", new StringContent(newcredentials.ToString(), Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject user = JObject.Parse(respon);
                    return (JObject)user["response"];
                }
                else
                    return null;
            }
        }

        public string status(string token, string id_site, string url_status)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string parameters = url_status + "?token=" + token + "&id_site=" + id_site;

                HttpResponseMessage response = client.GetAsync(parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject credentials = JObject.Parse(respon);
                    return credentials["response"].ToString();
                }
                else
                    return string.Empty;
            }
        }

        public string accounts(string token, string id_site)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string parameters = "accounts?token=" + token + "&id_site=" + id_site;

                HttpResponseMessage response = client.GetAsync(parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject credentials = JObject.Parse(respon);
                    return credentials["response"].ToString();
                }
                else
                    return string.Empty;
            }
        }

        public string transactions(string token, string id_account)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(paybook_link);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string parameters = "transactions?token=" + token + "&id_account=" + id_account;

                HttpResponseMessage response = client.GetAsync(parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string respon = response.Content.ReadAsStringAsync().Result;
                    JObject credentials = JObject.Parse(respon);
                    return credentials["response"].ToString();
                }
                else
                    return string.Empty;
            }
        }
    }

    public class data
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string api_key { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id_user { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string token { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id_site { get; set; }
    }
}
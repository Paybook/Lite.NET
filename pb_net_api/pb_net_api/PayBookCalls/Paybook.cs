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

        //public JObject credentials(string token, string id_site, string id_user, string credentials_user, string credentials_password)
        public JObject credentials(string token, string name, string id_site, string[] credentials_user)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(paybook_link + "credentials");
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            data2 data = new data2() { token = token, name = name, id_site = id_site, credentials = credentials_user };
            string jsonContent = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);

            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                // Log exception and throw as for GET example above
            }
            return null;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(paybook_link);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    //client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            //    // HTTP POST
            //    //data2 data = new data2() { api_key = api_key, id_user = id_user, id_site = id_site, credentials = new credentials { username = credentials_user, password = credentials_password } };

            //    //string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);

            //    data2 data = new data2() { token = token, name = name, id_site = id_site, credentials = new credentials { username = credentials_user, password = credentials_password } };
            //    //HttpResponseMessage response = client.PostAsJsonAsync("credentials", data).Result;

            //    //HttpResponseMessage response = client.PostAsJsonAsync("credentials", data).Result;
            //    //HttpResponseMessage response = client.PostAsJsonAsync("credentials", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            //    string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);

            //    //string parameters = @"credentials?token=" + token + "&name="+name+"&id_site="+id_site+"&credentials%5Busername%5D="+credentials_user+"&credentials%5Bpassword%5D="+credentials_password;
            //    HttpResponseMessage response = client.PostAsync("credentials", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string respon = response.Content.ReadAsStringAsync().Result;
            //        JObject user = JObject.Parse(respon);
            //        return (JObject)user["response"];
            //    }
            //    else
            //        return null;
            //}
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

    public class data2
    {
        public string token { get; set; }

        public string name { get; set; }

        public string id_site { get; set; }

        public string [] credentials { get; set; }
    }

    public class credentials
    {
        public string username { get; set; }

        public string password { get; set; }
    }
}
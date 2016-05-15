using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pb_net_api.Models
{
    public class CredentialsModel
    {
        public string id_user { get; set; }
        public string id_site { get; set; }
        public string ws { get; set; }
        public string status { get; set; }
        public string twofa { get; set; }
        public string id_credential { get; set; }
    }
}
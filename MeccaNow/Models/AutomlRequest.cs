using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeccaNow.Models
{
    public class AutomlRequest
    {
        public payload payload { get; set; }
    }

    public class payload
    {
        public image image { get; set; }
     
    }

    public class image
    {
        public string imageBytes { get; set; }
    }

    public class GooglePlusAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
}
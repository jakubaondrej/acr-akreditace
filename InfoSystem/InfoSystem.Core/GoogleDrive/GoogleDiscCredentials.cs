using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.GoogleDrive
{
    public class GoogleDiscCredentials
    {
        public string Client_id { get; set; }
        public string Project_id { get; set; }
        public string Auth_uri { get; set; }
        public string Token_uri { get; set; }
        public string Auth_provider_x509_cert_url { get; set; }
        public string Client_secret { get; set; }
        public string[] Redirect_uris { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string GoogleApiKey { get; set; }
        public string ServiceAccountEmail { get; set; }
        public string KeyFilePath { get; set; }
        
    }

}

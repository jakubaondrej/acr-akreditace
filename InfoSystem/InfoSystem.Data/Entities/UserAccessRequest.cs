using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class UserAccessRequest
    {
        public int UserAccessRequestId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string CallNumber { get; set; }
        public string Redaction { get; set; }
        public string Note { get; set; }
    }
}

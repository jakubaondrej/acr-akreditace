using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.UserAccessRequest
{
    public class UserAccessRequestDetailModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string CallNumber { get; set; }
        public string Redaction { get; set; }
        public string Note { get; set; }
    }
}

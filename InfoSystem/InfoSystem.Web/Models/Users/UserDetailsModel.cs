using InfoSystem.Web.Models.Redaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Users
{
    public class UserDetailsModel
    {
        public string Id { get; set; }
        public string Note { get; set; }
        public int? RedactionId { get; set; }
        public RedactionListingModel Redaction { get; set; }
        public string Username { get; set; }
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; }

    }
}

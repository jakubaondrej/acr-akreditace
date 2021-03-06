using InfoSystem.Web.Models.Redaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.UserAccessRequest
{
    public class AcceptUserAccessRequestModel
    {
        public string RedactionRequired { get; set; }
        [Display(Name = "Create as a new redaction.")]
        public bool CreateAsNewRedaction { get; set; }

        [Required]
        [Remote(action: "IsUserNameInUse", controller: "User")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "User")]
        public string Email { get; set; }
        public string Note { get; set; }

        [Display(Name = "User role"), Required]
        public string SelectedUserRole { get; set; }
        public IEnumerable<SelectListItem> UserRoleItems
        {
            get
            {
                var items = InfoSystem.Core.Users.Roles.GetAll().Select(a => new SelectListItem(a, a)).ToList();
                return items;
            }
        }

        [Display(Name = "Redaction"), Required]
        public int SelectedRedactionId { get; set; }
        public IEnumerable<RedactionListingModel> Redactions { get; set; }

        public IEnumerable<SelectListItem> RedactionItems
        {
            get
            {
                if (Redactions != null)
                {
                    var items = Redactions.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList();
                    return items;
                }
                return null;
            }
        }
        [Display(Name = "Redactor type")]
        public string RedactorType { get; set; }
        public IEnumerable<SelectListItem> RedactorTypeItems
        {
            get
            {
                var items = InfoSystem.Core.Users.RedactorType.GetAll().Select(a => new SelectListItem(a, a)).ToList();
                return items;
            }
        }
    }
}

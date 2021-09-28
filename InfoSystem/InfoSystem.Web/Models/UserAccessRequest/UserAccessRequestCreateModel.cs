using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.UserAccessRequest
{
    public class UserAccessRequestCreateModel
    {
        public bool isTrue
        { get { return true; } }
        [Required]
        [Display(Name = "I agree to the terms and conditions")]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool AgreeTerms { get; set; }
        [Required,Display(Name ="Uživatelské jméno (username)")]
        [Remote(action: "IsUserNameInUse", controller: "User")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "User")]
        public string Email { get; set; }
        [Display(Name ="Telefonní číslo (call number)")]
        public string CallNumber { get; set; }
        [Required]
        [Display(Name = "Redakce (redaction)")]
        public string Redaction { get; set; }
        [Display(Name ="Poznámka (note)")]
        public string Note { get; set; }
    }
}

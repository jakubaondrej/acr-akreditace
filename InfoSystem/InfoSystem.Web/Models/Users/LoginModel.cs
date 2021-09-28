using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Uživatelské jméno je vyžadováno / Username is required"), Display(Name = "Uživatelské jméno / Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

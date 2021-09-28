using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Users
{
    public class UserChangePasswordModel
    {
        [DataType(DataType.Password)]
        [Display(Name ="Old password",Description ="Insert your last password.")]
        [Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Remote(action: "IsUserNameInUse", controller: "User")]
        [DataType(DataType.Password)]
        [Display(Name ="New password",Description ="Insert new password to change.")]
        public string NewPassword { get; set; }
    }
}

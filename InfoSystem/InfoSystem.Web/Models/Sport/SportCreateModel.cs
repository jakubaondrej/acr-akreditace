using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Sport
{
    public class SportCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Image link")]
        public Uri ImageUri { get; set; }
    }
}

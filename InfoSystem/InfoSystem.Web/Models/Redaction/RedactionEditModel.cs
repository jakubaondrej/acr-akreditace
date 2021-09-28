using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Redaction
{
    public class RedactionEditModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "General editor name")]
        public string GeneralEditor { get; set; }
        [Display(Name = "General editor call number")]
        public string GeneralEditorCallNumber { get; set; }
        [Display(Name = "General editor email")]
        public string GeneralEditorEmail { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
    }
}

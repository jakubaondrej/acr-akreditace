using InfoSystem.Web.Models.Sport;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.championship
{
    public class ChampionshipCreateModel
    {
        [Required]
        public string Name { get; set; }
        public int SelectedSportId { get; set; }
        public List<SportListingModel> Sports { get; set; }
        public IEnumerable<SelectListItem> SportItems
        {
            get
            {
                if (Sports != null)
                {
                    var items = Sports.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList();
                    return items;
                }
                return null;
            }
        }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name="Image link")]
        public string ImageUrl { get; set; }
    }
}

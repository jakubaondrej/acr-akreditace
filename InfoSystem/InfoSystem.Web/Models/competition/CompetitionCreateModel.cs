using InfoSystem.Web.Models.championship;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.competition
{
    public class CompetitionCreateModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Championship"), Required]
        public int SelectedChampionshipId { get; set; }
        public IEnumerable<ChampionshipListingModel> Championships { get; set; }

        public IEnumerable<SelectListItem> ChampionshipItems
        {
            get
            {
                if (Championships != null)
                {
                    var items = Championships.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList();
                    return items;
                }
                return null;
            }
        }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Image link")]
        public Uri ImageUrl { get; set; }
    }
}

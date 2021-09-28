using InfoSystem.Web.Models.Sport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.championship
{
    public class ChampionshipDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SportListingModel Sport { get; set; }
        [Display(Name="Image")]
        public Uri ImageUrl { get; internal set; }
    }
}

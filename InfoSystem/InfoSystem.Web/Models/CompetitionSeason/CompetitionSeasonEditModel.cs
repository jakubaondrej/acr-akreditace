using InfoSystem.Web.Models.Season;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.CompetitionSeason
{
    public class CompetitionSeasonEditModel
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        [Required, DataType(DataType.Url)]
        [Display(Name = "Picture storage URL")]
        public Uri PictureStoreUri { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime End { get; set; }
        [Required, DataType(DataType.DateTime)]
        [Display(Name = "Journalist registration start")]
        public DateTime JournalistRegistrationStart { get; set; }
        [Required, DataType(DataType.DateTime)]
        [Display(Name = "Journalist registration end")]
        public DateTime JournalistRegistrationEnd { get; set; }
        [Required, DataType(DataType.DateTime)]
        [Display(Name = "Journalist upload foto deadline")]
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public List<SeasonListingModel> Seasons { get; set; }
        [Required]
        public int SelectedSeasonId { get; set; }
        public IEnumerable<SelectListItem> SeasonItems
        {
            get
            {
                if (Seasons != null)
                {
                    var items = Seasons.Select(a => new SelectListItem(a.Year.ToString(), a.SeasonId.ToString())).ToList();
                    return items;
                }
                return null;
            }
        }
    }
}

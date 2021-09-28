using InfoSystem.Web.Models.competition;
using InfoSystem.Web.Models.Season;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.CompetitionSeason
{
    public class CompetitionSeasonDetailsModel
    {
        public int Id { get; set; }
        [Display(Name = "Picture storage URL")]
        public Uri PictureStoreUri { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Journalist registration start")]
        public DateTime JournalistRegistrationStart { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Journalist registration end")]
        public DateTime JournalistRegistrationEnd { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Journalist upload foto deadline")]
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public SeasonListingModel Seasons { get; set; }
        public CompetitionListingModel Competition { get; set; }
    }
}

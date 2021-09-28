using InfoSystem.Web.Models.Accreditation;
using InfoSystem.Web.Models.Season;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.CompetitionSeason
{
    public class CompetitionSeasonAccreditationUserModel
    {
        public championship.ChampionshipListingModel Championship { get; set; }
        public Sport.SportListingModel Sport { get; set; }
        public competition.CompetitionListingModel Competition { get; set; }
        public SeasonListingModel Season { get; set; }
        public AccreditationListingModel Accreditation { get; set; }
        public int Id { get; set; }
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
        [Display(Name = "Official name")]
        public string OfficialSeasonName { get; set; }
    }
}

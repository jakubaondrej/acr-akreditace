using InfoSystem.Web.Models.competition;
using InfoSystem.Web.Models.Season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.CompetitionSeason
{
    public class CompetitionSeasonListingModel
    {
        public int CompetitionSeasonId { get; set; }
        public string OfficialSeasonName { get; set; }
        public string PictureUrl { get; set; }
        public SeasonListingModel Season { get; set; }
    }
}
